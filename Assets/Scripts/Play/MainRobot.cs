using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテムを使用して力を加えて動く、ゲームのメインロボット
[RequireComponent(typeof(RobotStatus))]
[RequireComponent(typeof(ForceMove))]
public class MainRobot : MonoBehaviour
{
    RobotStatus _status;
    ForceMove _move;

    // アイテムを強制的に使用するかのフラグ（リプレイなどで整合性が崩れないように）
    private bool IsUsePartsInForce = false;
    // リプレイ操作に従うか
    // private bool ReplayMode = false;


    private void Awake()
    {
        _status = GetComponent<RobotStatus>();
        _move = GetComponent<ForceMove>();
    }

    // Update is called once per frame
    private void Update()
    {
        // 仮の動作テスト処理（アイテム使用）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseParts();
        }
    }

    // ゲーム開始メソッド
    [ContextMenu("Debug/GameStart")]
    public void GameStart()
    {
        // ロボットの初期重量を設定する
        float allWeight = PlayPartsManager.Instance.GetAllWeight();
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
        PartsInfo.PartsData data;
        IForce force;
        PlayPartsManager.Instance.UseParts(out data, out force);

        // 状態管理にアイテムの使用を伝える
        _status.startUseParts(data);

        // 物理管理に力を加える
        _move.AddForce(force);
    }
}
