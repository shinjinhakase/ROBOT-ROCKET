using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテムを使用して力を加えて動く、ゲームのメインロボット
[RequireComponent(typeof(ForceMove))]
[RequireComponent(typeof(ReplayPlayer))]
public class MainRobot : MonoBehaviour
{
    private static string GameOverColliderTag = "GameOverCollider"; // ゲームオーバーとなる当たり判定に付けるタグの名前

    private ReplayPlayer _player;
    private PartsInfo partsInfo;
    private PlaySceneController playSceneController;
    private PlayPartsManager playPartsManager;
    public RobotStatus _status;
    [HideInInspector] public ForceMove _move;

    private ReplayData _useReplayData;  // リプレイ再生時に用いるデータ
    private float _highScore = 0;       // 最高到達距離
    [SerializeField] private float _underline;  // 超えたらゲームオーバーとなる基準高度

    [SerializeField] private ParticleSystem _gameoverPerticle;

    // シーン判定
    private bool IsWaitForControl => playSceneController ? playSceneController.IsWaitingForRobot && !playSceneController.IsReplayMode : false;
    private bool IsControlMode => playSceneController ? playSceneController.IsRobotStartMove && playSceneController.IsPlayingGame && !playSceneController.IsReplayMode : false;
    private bool IsReplayMode => playSceneController ? playSceneController.IsReplayMode : throw new NullReferenceException("PlaySceneControllerのAwake処理前にInstanceを参照しました");

    private void Awake()
    {
        partsInfo = PartsInfo.Instance;
        _move = GetComponent<ForceMove>();
        _player = GetComponent<ReplayPlayer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // 飛行中の操作モード時処理
        if (IsControlMode)
        {
            // アイテム使用終了判定
            if (_status.IsUsingParts && !playPartsManager.IsUsingParts)
            {
                endUseParts();
            }

            // アイテム使用キーの入力受付
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UsePartsByControl();
            }
            // アイテムの手動パージ処理
            if (playPartsManager.IsUsingParts && Input.GetKeyDown(KeyCode.R))
            {
                playPartsManager.IsUsingParts = false;
            }

            // 高度によるゲームオーバー判定
            if (transform.position.y <= _underline)
            {
                playSceneController.GameOver();
            }
        }
        // 飛行し始め判定
        else if (IsWaitForControl && partsInfo.HasNext && Input.GetKeyDown(KeyCode.Space))
        {
            RobotStartMove();
            UsePartsByControl();
        }
    }

    private void FixedUpdate()
    {
        // 最高到達距離を確認する
        if (IsControlMode && transform.position.x > _highScore)
        {
            _highScore = transform.position.x;
            playSceneController.Score = _highScore;
            // クリア判定
            if (_highScore >= playSceneController.GoalXPoint)
            {
                playSceneController.GameClear();
            }
        }
    }

    // ゲーム開始メソッド
    public void GameStart()
    {
        // キャッシュを取得
        playSceneController = PlaySceneController.Instance;
        playPartsManager = PlayPartsManager.Instance;

        if (IsReplayMode)
        {
            // リプレイの初期設定
            if (_useReplayData == null) throw new Exception("リプレイ用のデータが設定されていません。");
            _player.LoadReplayData(_useReplayData);

            // 用意してきたパーツをリプレイのものに変更
            partsInfo.partsList = _player.InitialPartsDatas;

            // スコアをリプレイのものに変更
            playSceneController.Score = _useReplayData.score;

            // リプレイ時は1秒後に動き始める
            Invoke("RobotStartMove", 1f);
        }
        else
        {
            // リプレイ時に使用するデータを初期化する
            _useReplayData = null;
        }
    }
    // ロボットが動き始めた際に呼ばれるメソッド
    private void RobotStartMove()
    {
        // ロボットの初期重量を設定する
        float allWeight = playPartsManager.GetAllWeight();
        _move.SetWeight(allWeight + ForceMove.RobotWeight);

        // 状態を変化させる
        _status.startGame();

        // ロボットが動き始めた際の処理状態に移る
        playSceneController.RobotStartMove();
        if (_player.IsLoaded) _player.StartReplay();
    }

    // 操作によってアイテムを使用する処理
    [ContextMenu("Debug/UseParts")]
    private void UsePartsByControl()
    {
        if (playSceneController.IsReplayMode) return;
        else if (!partsInfo.HasNext) return;
        else if (!_status.IsPartsUsable) return;
        playPartsManager.UseParts(out PartsPerformance performance, out PartsInfo.PartsData data, out IForce force);
        UseParts(data, performance, force);
    }
    // リプレイによってアイテムを使用する処理
    public void UsePartsByReplay(PartsInfo.PartsData data)
    {
        if (!playSceneController.IsReplayMode) return;
        playPartsManager.UseParts(out PartsPerformance performance, out _, out _);
        UseParts(data, performance);
    }
    // パーツ使用時の共通処理
    private void UseParts(PartsInfo.PartsData data, PartsPerformance performance, IForce force = null)
    {
        // アイテムが使えるか判定
        if (!_status.IsFlying)
        {
            Debug.LogWarning("飛行中以外にアイテムを使用しようとしています！");
            return;
        }
        // アイテムが使用できない状態のとき
        else if (!_status.IsPartsUsable && IsReplayMode)
        {
            // リプレイ時はアイテムを使用できる状態に強制的に移行する
            if (_status.IsUsingParts) _status.endUseParts();
            _status.endCooltime();
        }

        // 状態管理にアイテムの使用を伝える
        _status.startUseParts(performance, data);

        // 物理管理に力を加える
        _move.AddForce(force);

        // 召喚オブジェクトを召喚する
        Transform _transform = transform;
        Vector3 nowPosition = _transform.position;
        foreach (SummonableObject summonObject in performance.summonObjects)
        {
            var summonned = Instantiate(summonObject, nowPosition, Quaternion.identity);
            summonned.Summon(data, _transform);
            if (summonned.IsDestroyWithParts)
            {
                // 設定がオンであれば、パーツとともに破棄されるように登録する
                _status.RegisterObjectAsDestroyWithParts(summonned.gameObject);
            }
        }

        Debug.Log("(MainRobot)パーツを使用しました：ID = " + data.id);
    }
    // パーツの使用が終わった際の処理
    public void endUseParts()
    {
        PartsInfo.Instance.RemoveParts();   // パーツをリストから削除する
        _status.endUseParts();
        Debug.Log("(MainRobot)パーツの使用を終了しました");
    }


    // ゲームクリア時の処理
    public void GameClear()
    {
        // 力を無くし、成功アニメーション処理に遷移する
        _move.ZeroForce();
        _status.GameClear();
    }

    // ゲームオーバー時の処理
    public void GameOver()
    {
        // 失敗アニメーション処理に遷移する
        _move.ZeroForce();
        _status.GameOver();

        // エフェクトを表示する
        var effect = Instantiate(_gameoverPerticle, transform.position, Quaternion.Euler(-90, 0, 0));
        GimickManager.Instance.RegisterAsDeleteObject(effect.gameObject);

        // ロボットを非表示にする（パージのパーツが飛び散るアニメーションに移る）
        gameObject.SetActive(false);
    }

    // カスタムメニューを開いたときの処理
    public void OpenCustomMenu()
    {
        // 飛行中に呼び出されたなら、ゲームオーバー時の爆破処理を呼ぶ
        if (playSceneController ? playSceneController.IsRobotStartMove && _status.IsFlying : false)
        {
            GameOver();
        }
        // （ゲーム終了後に呼び出されたなら何もしない）
    }

    // スタート地点に戻り、状態をリセットする
    public void ResetToStart()
    {
        _highScore = 0;
        partsInfo = PartsInfo.Instance;
        _move.ResetToFirst();
        gameObject.SetActive(true);
        _status.ResetStatus();
    }

    // リプレイを再生する際の準備
    public void SetReplayData()
    {
        if (_useReplayData == null)
        {
            // リプレイデータが空なら、現在のリプレイデータを取得する
            _useReplayData = ReplayInputManager.Instance.Data;
        }
    }


    // ゲームオーバーとなる当たり判定との衝突判定を担うメソッド
    public void CheckGameOverCollision(Collider2D other)
    {
        if (IsControlMode && other.CompareTag(GameOverColliderTag))
        {
            if (other.TryGetComponent(out GameOverSE gameoverSE))
            {
                // ゲームオーバー用のSEが設定されているなら鳴らす。
                gameoverSE.Beep();
            }
            PlaySceneController.Instance.GameOver();
        }
    }
    // リプレイ用に自分の位置と速度情報を返答する
    public void GetTransform(out Vector2 position, out Vector2 velocity)
    {
        position = transform.position;
        velocity = _move.GetVelocity();
    }
}
