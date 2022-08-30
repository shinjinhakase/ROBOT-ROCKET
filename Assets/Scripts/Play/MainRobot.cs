using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテムを使用して力を加えて動く、ゲームのメインロボット
[RequireComponent(typeof(RobotStatus))]
[RequireComponent(typeof(ForceMove))]
public class MainRobot : MonoBehaviour
{
    PartsInfo partsInfo;
    PlayPartsManager playPartsManager;
    RobotStatus _status;
    ForceMove _move;

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
        }
        if (Input.GetKey(KeyCode.A))
        {
            Time.timeScale = 0.25f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    // ゲーム開始メソッド
    [ContextMenu("Debug/GameStart")]
    public void GameStart()
    {
        // ロボットの初期重量を設定する
        playPartsManager = PlayPartsManager.Instance;
        float allWeight = playPartsManager.GetAllWeight();
        _move.SetWeight(allWeight + ForceMove.RobotWeight);

        // 状態を変化させる
        _status.startGame();
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
}
