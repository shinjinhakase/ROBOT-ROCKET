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

    private float _highScore = 0;   // 最高到達距離

    private bool IsNotStart = false;        // 飛行し始めたタイミングを計るための、飛行していないかフラグ
    private bool IsUsePartsInForce = false; // アイテムを強制的に使用するかのフラグ（リプレイなどで整合性が崩れないように）
    private bool ReplayMode = false;        // リプレイ操作に移るかのフラグ
    private ReplayData _useReplayData;

    private void Awake()
    {
        partsInfo = PartsInfo.Instance;
        _move = GetComponent<ForceMove>();
        _player = GetComponent<ReplayPlayer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // 飛行中の処理
        if (_status.IsFlying && !_player.IsPlaying)
        {
            // アイテム使用終了判定
            if (_status.IsUsingParts && !playPartsManager.IsUsingParts)
            {
                endUseParts();
            }

            // 仮の操作処理（アイテム使用）
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("アイテム使用ボタンを押した");
                UsePartsByControl();
            }
            // アイテムの手動パージ
            if (playPartsManager.IsUsingParts && Input.GetKeyDown(KeyCode.R))
            {
                playPartsManager.IsUsingParts = false;
            }
        }
        // 飛行し始め判定
        else if (IsNotStart &&  playSceneController.scene == PlaySceneController.E_PlayScene.GamePlay && _status.IsWaitingForFly && Input.GetKeyDown(KeyCode.Space))
        {
            IsNotStart = false;
            RobotStartMove();
            UsePartsByControl();
        }

        // ヒットストップデバッグ
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlaySceneController.Instance.RequestHitStopBySlow(0.25f, 1f);
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            PlaySceneController.Instance.RequestHitStopByStop(1f);
        }
    }

    private void FixedUpdate()
    {
        // 最高到達距離を確認する
        if (_status.IsFlying && !_player.IsPlaying && transform.position.x > _highScore)
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

        if (ReplayMode)
        {
            // リプレイの初期設定
            if (_useReplayData == null) throw new Exception("リプレイ用のデータが設定されていません。");
            _player.LoadReplayData(_useReplayData);

            // 用意してきたパーツをリプレイのものに変更
            partsInfo.partsList = _player.InitialPartsDatas;

            // スコアをリプレイのものに変更
            playSceneController.Score = _useReplayData.score;

            // リプレイ用の処理モードに変更
            IsUsePartsInForce = true;
            IsNotStart = false;
            Invoke("RobotStartMove", 1f);
        }
        else
        {
            // 操作する際の処理
            IsUsePartsInForce = false;
            IsNotStart = true;  // まだ飛行を開始していないフラグをtrueにする
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

        // ロボットが動き始めた際のイベントを呼ぶ
        playSceneController.RobotStartMove();
        if (_player.IsLoaded) _player.StartReplay();
    }

    // 操作によってアイテムを使用する処理
    [ContextMenu("Debug/UseParts")]
    private void UsePartsByControl()
    {
        if (_player.IsPlaying) return;
        else if (!partsInfo.HasNext) return;
        else if (!_status.IsPartsUsable) return;
        playPartsManager.UseParts(out PartsPerformance performance, out PartsInfo.PartsData data, out IForce force);
        UseParts(data, performance, force);
    }
    // リプレイによってアイテムを使用する処理
    public void UsePartsByReplay(PartsInfo.PartsData data)
    {
        if (!_player.IsPlaying) return;
        UseParts(data, playPartsManager.GetPerformance(data.id));
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
        else if (!_status.IsPartsUsable && IsUsePartsInForce)
        {
            // アイテムを使用できる状態に強制的に移行する
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
        }
    }
    // パーツの使用が終わった際の処理
    public void endUseParts()
    {
        _status.endUseParts();
    }


    // ゲームクリア時の処理
    public void GameClear()
    {
        ReplayMode = false;
        // 力を無くし、成功アニメーション処理に遷移する
        _move.ZeroForce();
        _status.GameClear();
    }

    // ゲームオーバー時の処理
    public void GameOver()
    {
        ReplayMode = false;
        // 失敗アニメーション処理に遷移する
        _move.ZeroForce();
        _status.GameOver();

        // ロボットを非表示にする（パージのパーツが飛び散るアニメーションに移る）
        gameObject.SetActive(false);
    }

    // カスタムメニューを開いたときの処理
    public void OpenCustomMenu()
    {
        // 飛行中に呼び出されたなら、クレーンで持ち上げられるアニメーションを入れる
        if (_status.IsFlying)
        {
            ReplayMode = false;
            _player.StopReplayInForce();
            _move.OpenCustomMove();
            _status.OpenCustomMenu();
        }
        // （ゲームオーバー後に呼び出されたなら、既に非表示なので何もしない）
    }

    // リセットするときの処理
    public void ResetToStart()
    {
        _highScore = 0;
        partsInfo = PartsInfo.Instance;
        _move.ResetToFirst();
        _status.ResetStatus();
        gameObject.SetActive(true);
    }

    // リプレイを再生する際の準備
    public void SetReplayMode()
    {
        ReplayMode = true;
        if (_useReplayData == null)
        {
            // リプレイデータが空なら、現在のリプレイデータを取得する
            _useReplayData = new ReplayData(ReplayInputManager.Instance.Data);
        }
    }


    // ゲームオーバーとなる当たり判定との衝突判定を担うメソッド
    public void CheckGameOverCollision(Collider2D other)
    {
        if (!_player.IsPlaying && _status.IsFlying && other.CompareTag(GameOverColliderTag))
        {
            PlaySceneController.Instance.GameOver();
        }
    }
    // リプレイに自分の位置と速度情報を格納する
    public void GetTransform(out Vector2 position, out Vector2 velocity)
    {
        position = transform.position;
        velocity = _move.GetVelocity();
    }
}
