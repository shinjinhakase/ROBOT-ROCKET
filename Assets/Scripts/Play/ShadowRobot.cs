using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シャドウの動きを処理するComponent（動きの軌跡だけ再現する）
[RequireComponent(typeof(RobotStatus))]
[RequireComponent(typeof(ForceMove))]
public class ShadowRobot : MonoBehaviour
{
    // リプレイデータ
    private int replayNo = -1;
    private int readyPartsLength = 0;
    private int getPartsLength = 0;
    private int transformLength = 0;
    private ReplayData replayData;

    // リプレイ用変数
    private bool IsStart = false;   // 開始フラグ
    private int frameCnt = 0;   // フレームカウンタ

    // 使用パーツ関連
    private PartsInfo.PartsData GetPartsData(int index) => index < readyPartsLength ? replayData.readyPartsList[index] : replayData.getPartsList[index - readyPartsLength].buildPartsData();
    private int partsNo = 0;    // 使用しているパーツの数
    private bool IsUsingParts => partsNo < readyPartsLength + getPartsLength && frameCnt >= replayData.usePartsFrame[partsNo];  // パーツを使用するフレームか判定

    // 位置情報関連
    private int transCnt = 0;   // 位置情報の参照インデックス
    private ReplayData.LocateData nextLocateData = null;
    private ReplayData.LocateData GetNextLocate() => transCnt < transformLength ? replayData.locateDatas[transCnt] : null;  // 次の更新データを取得
    private bool IsTransformUpdating => nextLocateData != null && frameCnt >= nextLocateData.frame; // 座標が更新されるフレームか判定

    // 力関連
    private List<ReplayData.ForceData> GetAddForces() => replayData.forceDatas.FindAll(data => frameCnt == data.frame); // このフレームで加えられる力のデータを取得

    // キャッシュ
    private RobotStatus _status;
    private ForceMove _move;
    private PlayPartsManager _playPartsManager;
    private Rigidbody2D _rb;

    void Awake()
    {
        _status = GetComponent<RobotStatus>();
        _move = GetComponent<ForceMove>();
        _move.IsAcceptExternalForce = false;    // リプレイ以外の外力は誤差の元となるため受け入れない設定にする
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsStart)
        {
            // パーツ使用モーション遷移の処理
            if (IsUsingParts) UseParts();

            // 位置・速度などの定期的な修正
            if (IsTransformUpdating)
            {
                // データに基づいて位置と速度を修正する
                transform.position = nextLocateData.position;
                _rb.velocity = nextLocateData.velocity;
                transCnt++;
                nextLocateData = GetNextLocate();
            }

            // パーツ獲得時の質量変化処理
            AddGettingPartsWeight();
            // 力関係の処理
            var forceDatas = GetAddForces();
            if (forceDatas?.Count > 0)
            {
                // 加えられた力をデータから復元してForceMoveに与える
                foreach(var forceData in forceDatas)
                {
                    var force = forceData.buildForce();
                    _move.AddForce(force);
                }
            }

            // リプレイの終了処理
            if(frameCnt >= replayData.finishFrame)
            {
                ReplayFinish();
            }

            frameCnt++;
        }
    }

    // リプレイデータを読み込む
    public void LoadReplayData(int index)
    {
        // リプレイデータを取得する
        ReplayDatas replayDatas = ReplayDatas.Instance;
        if (index >= replayDatas.Length)
        {
            throw new Exception("リプレイデータが読み込めません。");
        } else if (replayNo != -1)
        {
            throw new Exception("既にリプレイデータを読み込んでいます。");
        }
        replayNo = index;
        replayData = replayDatas.GetData(index);

        // パーツ情報の取得
        readyPartsLength = replayData.readyPartsList.Count; // 準備してきたパーツの数を取得
        getPartsLength = replayData.getPartsList.Count;     // 道中で獲得したパーツの数を取得
        partsNo = 0;
        // 位置・力情報の取得
        transformLength = replayData.locateDatas.Count;     // 更新位置情報の件数を取得
        transCnt = 0;
        nextLocateData = GetNextLocate();                   // 次に更新する座標・速度のデータを取得
        frameCnt = 0;
    }

    // リプレイの再現を開始する
    public void StartGame()
    {
        // 準備していたパーツの初期重量をロボットに適用
        _playPartsManager = PlayPartsManager.Instance;
        SetInitialWeight();

        _status.startGame();
        IsStart = true;
    }

    // パーツを使用していたフレームで呼び出されるメソッド
    private void UseParts()
    {
        // 使用したパーツの性能・カスタムデータを取得する
        PartsInfo.PartsData data = GetPartsData(partsNo);
        PartsPerformance performance = _playPartsManager.GetPerformance(data.id);

        // パーツの使用状態に移る（アニメーション遷移）
        _status.startUseParts(performance, data);

        partsNo++;
    }

    // リプレイ終了時の処理
    public void ReplayFinish()
    {
        // 力を無くし、ゲームオーバーモーションを出した上で自身を削除する
        _move.ZeroForce();
        _status.GameOver();
        IsStart = false;    // どうせ破棄されるけど一応
        ShadowManager.Instance.FinishReplay(this);
    }



    // 開始時の初期質量を設定する
    private void SetInitialWeight()
    {
        float sumWeight = 0;
        foreach (var readyPartsData in replayData.readyPartsList)
        {
            sumWeight += _playPartsManager.GetPerformance(readyPartsData.id).m;
        }
        _move.SetWeight(sumWeight + ForceMove.RobotWeight);
    }

    // 現在のフレームで取得したパーツの重量をロボットに追加する
    private void AddGettingPartsWeight()
    {
        // 現在のフレームで獲得したパーツのデータを取得
        List<ReplayData.GetPartsData> partsDatas = replayData.getPartsList.FindAll(data => frameCnt == data.frame);
        if (partsDatas?.Count == 0) return;

        // 取得したパーツの総重量を計算する
        float sumWeight = 0;
        foreach(var data in partsDatas)
        {
            sumWeight += _playPartsManager.GetPerformance(data.id).m;
        }

        // ロボットの重量を更新する
        _move.SetWeight(_move.GetWeight() + sumWeight);
    }
}
