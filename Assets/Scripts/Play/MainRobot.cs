using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテムを使用して力を加えて動く、ゲームのメインロボット
[RequireComponent(typeof(RobotStatus))]
[RequireComponent(typeof(ForceMove))]
public class MainRobot : MonoBehaviour
{
    private static string GameOverColliderTag = "GameOverCollider"; // ゲームオーバーとなる当たり判定に付けるタグの名前

    private PartsInfo partsInfo;
    private ReplayInputManager replayInputManager;
    private PlayPartsManager playPartsManager;
    public RobotStatus _status;
    public ForceMove _move;

    // アイテムを強制的に使用するかのフラグ（リプレイなどで整合性が崩れないように）
    private bool IsUsePartsInForce = false;
    // リプレイ操作に従うか
    // private bool ReplayMode = false;


    private void Awake()
    {
        partsInfo = PartsInfo.Instance;
        _status = GetComponent<RobotStatus>();
        _move = GetComponent<ForceMove>();
    }

    // Update is called once per frame
    private void Update()
    {
        // 飛行中の処理
        if (_status.IsFlying)
        {
            // アイテム使用終了判定
            if (_status.IsUsingParts && !playPartsManager.IsUsingParts) _status.endUseParts();

            // 仮の操作処理（アイテム使用）
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("アイテム使用ボタンを押した");
                UseParts();
            }
            // アイテムの手動パージ（グライダーでは実装？他のパーツではどうするか聞いてない）
            if(playPartsManager.IsUsingParts && Input.GetKeyDown(KeyCode.R))
            {
                playPartsManager.IsUsingParts = false;
            }
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

    // ゲーム開始メソッド
    public void GameStart()
    {
        // ロボットの初期重量を設定する
        replayInputManager = ReplayInputManager.Instance;
        playPartsManager = PlayPartsManager.Instance;
        float allWeight = playPartsManager.GetAllWeight();
        _move.SetWeight(allWeight + ForceMove.RobotWeight);

        // 状態を変化させる
        _status.startGame();

        // 開始と同時にリプレイのフレームカウントをスタートさせる（仮の処理）
        replayInputManager.StartMemory();
    }

    // アイテムを使用する処理
    [ContextMenu("Debug/UseParts")]
    public void UseParts()
    {
        // アイテムが使えるか判定
        if (!_status.IsFlying)
        {
            Debug.LogWarning("飛行中以外にアイテムを使用しようとしています！");
            return;
        }
        else if (!partsInfo.HasNext) return;
        else if (!_status.IsPartsUsable)
        {
            // アイテムが使用できない状態のとき
            if (!IsUsePartsInForce) return;
            else
            {
                // アイテムを使用できる状態に強制的に移行する
                if (_status.IsUsingParts) _status.endUseParts();
                _status.endCooltime();
            }
        }

        // アイテム管理にアイテムの使用を伝え、必要な情報を貰う
        PartsPerformance performance;
        PartsInfo.PartsData data;
        IForce force;
        playPartsManager.UseParts(out performance, out data, out force);

        // 状態管理にアイテムの使用を伝える
        _status.startUseParts(performance, data);

        // 物理管理に力を加える
        _move.AddForce(force);

        // 召喚オブジェクトを召喚する
        Transform _transform = transform;
        Vector3 nowPosition = _transform.position;
        foreach(SummonableObject summonObject in performance.summonObjects)
        {
            var summonned = Instantiate(summonObject, nowPosition, Quaternion.identity);
            summonned.Summon(data, _transform);
        }
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

        // ロボットを非表示にする（パージのパーツが飛び散るアニメーションに移る）
        gameObject.SetActive(false);
    }
    // カスタムメニューを開いたときの処理
    public void OpenCustomMenu()
    {
        // 飛行中に呼び出されたなら、クレーンで持ち上げられるアニメーションを入れる
        if (_status.IsFlying)
        {
            _move.ZeroForce();
            _status.OpenCustomMenu();
        }
        // （ゲームオーバー後に呼び出されたなら、既に非表示なので何もしない）
    }
    // リセットするときの処理
    public void ResetToStart()
    {
        partsInfo = PartsInfo.Instance;
        _move.ResetToFirst();
        _status.ResetStatus();
        gameObject.SetActive(true);
    }

    // ゲームオーバーとなる当たり判定との衝突判定を担うメソッド
    public void CheckGameOverCollision(Collider2D other)
    {
        if (_status.IsFlying && other.CompareTag(GameOverColliderTag))
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
