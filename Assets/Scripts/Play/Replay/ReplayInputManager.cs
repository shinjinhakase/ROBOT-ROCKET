using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// 現在のリプレイ用のデータを格納する
public class ReplayInputManager : SingletonMonoBehaviourInSceneBase<ReplayInputManager>
{
    // 現在のプレイのリプレイデータ
    [SerializeField] private ReplayData _data = new ReplayData();
    public ReplayData Data => _data;
    [SerializeField] private int TransformUpdateFrame = 50; // 位置情報を更新する間隔（FixedUpdate単位でのフレーム）
    private int frameCnt = -1;

    private PlaySceneController _sceneController;
    [SerializeField] private MainRobot robot;

    private bool NoMemoryMode = false;
    private bool IsGamePlay = false;

    private void FixedUpdate()
    {
        if (IsGamePlay) frameCnt++;
    }

    // リプレイデータを保存せず、現在あるデータをそのままにする状態にする
    public void SetNoMemoryMode()
    {
        if (IsGamePlay) return;
        NoMemoryMode = true;
    }
    // データの初期化（シーン管理でゲームを開始したと同時に呼び出され、使用パーツ情報が初期化される）
    public void Ready()
    {
        _sceneController = PlaySceneController.Instance;
        if (NoMemoryMode) return;
        _data.ReadyPartsInfo(_sceneController.StageNum);
    }
    // リプレイの記録を開始する（ロボットの動き始めにフレームカウントを合わせたりできる）
    public void StartMemory()
    {
        frameCnt = 0;
        IsGamePlay = true;
        if (NoMemoryMode) return;
        StartCoroutine(GetRobotTransformInterval());
    }
    // パーツの使用を記録（パーツ管理がパーツを使用した際に呼び出し）
    public void UseParts()
    {
        if (NoMemoryMode) return;
        _data.RegisterUseParts(frameCnt);
    }
    // パーツの使用終了を記録（ロボットの状態管理が使用終了状態に遷移した際に呼び出し）
    public void EndUseParts()
    {
        if (NoMemoryMode) return;
        _data.RegisterEndUseParts(frameCnt);
    }
    // パーツの獲得を記録（パーツ管理がパーツを獲得した際に呼び出し）
    public void GetParts(PartsInfo.PartsData data)
    {
        if (NoMemoryMode) return;
        _data.RegisterGetParts(frameCnt, data.id, data.angle);
    }
    // 力を記録（力クラスが力を止めた時の自身、あるいはゲームが終了した時の残った力を記録）
    public void SetForce(IForce force)
    {
        if (NoMemoryMode) return;
        _data.RegisterRobotForce(frameCnt, force);
    }
    // ゲーム結果を記録（ゲームが終了した際に記録）
    public void SetResult()
    {
        if (!NoMemoryMode && !PlaySceneController.Instance.IsWaitingForRobot)
        {
            // リプレイモードではなく、ロボットが動き始めていた時は、取得したデータを自動で保存させてみる
            _data.RegisterResult(frameCnt, _sceneController.Score);
            Save();
        }
        IsGamePlay = false;
        NoMemoryMode = false;
    }
    // リプレイデータを保存する
    [ContextMenu("Debug/Save")]
    public void Save()
    {
        // ロボットが動き始めていないならデータを保存しない
        ReplayDatas datas = ReplayDatas.Instance;
        datas.RegisterData(_data);
        datas.Save();
    }

    // 定期的にロボット位置を記録する
    private IEnumerator GetRobotTransformInterval()
    {
        var waitForSeconds = new WaitForSeconds(TransformUpdateFrame * Time.fixedDeltaTime);
        while (true)
        {
            if (!_sceneController.IsPlayingGame)
            {
                yield break;
            }
            robot.GetTransform(out Vector2 position, out Vector2 velocity);
            _data.RegisterRobotTransform(frameCnt, position, velocity);
            yield return waitForSeconds;
        }
    }

    public string GetTweetText()
    {
        float finishTime = _data.finishFrame * Time.fixedDeltaTime;
        return "ステージ" + _data.StageNum + "にて、" + _data.readyPartsList.Count + "個のパーツを用いて" + finishTime + "秒で" + _data.score + "mまで飛べました！\n";
    }
}
