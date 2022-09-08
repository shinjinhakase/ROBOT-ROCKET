using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シャドウの動きを処理するComponent（動きの軌跡だけ再現する）
[RequireComponent(typeof(ForceMove))]
[RequireComponent(typeof(ReplayPlayer))]
public class ShadowRobot : MonoBehaviour
{
    // キャッシュ
    [SerializeField] private RobotStatus _status;
    private ForceMove _move;
    private ReplayPlayer _player;
    private PlayPartsManager _playPartsManager;

    void Awake()
    {
        _move = GetComponent<ForceMove>();
        _move.IsAcceptExternalForce = false;    // リプレイ以外の外力は誤差の元となるため受け入れない設定にする
        _player = GetComponent<ReplayPlayer>();
    }

    // リプレイデータを読み込む
    public void LoadReplayData(int index)
    {
        _player.LoadReplayData(index);
    }

    // リプレイの再現を開始する
    public void StartGame()
    {
        // 準備していたパーツの初期重量をロボットに適用
        _playPartsManager = PlayPartsManager.Instance;
        SetInitialWeight();

        _status.startGame();
        _player.StartReplay();
    }

    // パーツを使用タイミングで呼び出されるメソッド
    public void UseParts(PartsInfo.PartsData data)
    {
        // 使用したパーツの性能を取得する
        PartsPerformance performance = _playPartsManager.GetPerformance(data.id);

        // パーツの使用状態に移る（アニメーション遷移）
        if (_status.IsUsingParts)
        {
            Debug.LogWarning("ReplayPlayerの外部で強制的にパーツの使用を停止しました。");
            _status.endUseParts();
        }
        _status.endCooltime();
        _status.startUseParts(performance, data);
    }

    // パーツの使用終了タイミングで呼び出されるメソッド
    public void endUseParts()
    {
        _status.endUseParts();
    }

    // 取得したパーツの重量をロボットに追加する
    public void AddGettingPartsWeight(PartsInfo.PartsData data)
    {
        // 獲得したパーツの重量を取得する
        float weight = _playPartsManager.GetPerformance(data.id).m;

        // ロボットの重量を更新する
        _move.SetWeight(_move.GetWeight() + weight);
    }

    // リプレイ終了時の処理
    public void ReplayFinish()
    {
        // 力を無くし、ゲームオーバーモーションを出した上で自身を削除する
        _move.ZeroForce();
        _status.GameOver();
        ShadowManager.Instance.FinishReplay(this);
    }


    // 開始時の初期質量を設定する
    private void SetInitialWeight()
    {
        _move.SetWeight(_player.GetInitialWeight(_playPartsManager));
    }
}
