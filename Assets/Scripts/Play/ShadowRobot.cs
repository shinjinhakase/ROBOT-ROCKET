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
    private ReplayData replayData;

    // リプレイ用変数
    private bool IsStart = false;   // 開始フラグ
    private int frameCnt = 0;   // フレームカウンタ
    private int partsNo = 0;    // 使用しているパーツの数
    private bool IsUsingParts => partsNo < readyPartsLength + getPartsLength && frameCnt == replayData.usePartsFrame[partsNo];


    private RobotStatus _status;
    private ForceMove _move;
    private PlayPartsManager _playPartsManager;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<RobotStatus>();
        _move = GetComponent<ForceMove>();
        _move.IsAcceptExternalForce = false;    // リプレイ以外の外力は誤差の元となるため受け入れない設定にする
    }

    private void FixedUpdate()
    {
        if (IsStart)
        {
            // パーツ使用モーション遷移の処理
            if (IsUsingParts) UseParts();

            // TODO：位置・速度などの修正

            // TODO：力関係の処理

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
        ReplayDatas replayDatas = ReplayDatas.Instance;
        if (index >= replayDatas.Length)
        {
            throw new Exception("リプレイデータが読み込めません。");
        } else if (replayNo != -1)
        {
            throw new Exception("既にリプレイデータを読み込んでいます。");
        }
        // リプレイデータを取得する
        replayNo = index;
        replayData = replayDatas.GetData(index);
        readyPartsLength = replayData.readyPartsList.Count; // 準備してきたパーツの数を取得
        getPartsLength = replayData.getPartsList.Count;     // 道中で獲得したパーツの数を取得
        frameCnt = 0;
    }

    // リプレイでの再現を開始する
    public void StartGame()
    {
        _playPartsManager = PlayPartsManager.Instance;
        _status.startGame();
        IsStart = true;
    }

    // パーツを使用する
    private void UseParts()
    {
        // 使用したパーツの性能・カスタムデータを取得する
        PartsInfo.PartsData data;
        if (partsNo < readyPartsLength)
        {
            // 用意していたパーツを使用する
            data = replayData.readyPartsList[partsNo];
        }
        else
        {
            // 道中で入手したパーツを使用する
            data = replayData.getPartsList[partsNo - readyPartsLength].buildPartsData();
        }
        PartsPerformance performance = _playPartsManager.GetPerformance(data.id);

        // パーツの使用状態に移る（アニメーション遷移）
        //（移動に関しては他の力と一括で管理する）
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
}
