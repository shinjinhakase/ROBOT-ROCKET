using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 現在のリプレイ用のデータを格納する
public class ReplayInputManager : SingletonMonoBehaviourInSceneBase<ReplayInputManager>
{
    // 現在のプレイのリプレイデータ
    [SerializeField] private ReplayData _data = new ReplayData();
    [SerializeField] private int TransformUpdateFrame = 50; // 位置情報を更新する間隔（FixedUpdate単位でのフレーム）
    private int frameCnt = -1;

    private PlaySceneController _sceneController;
    [SerializeField] private MainRobot robot;


    private void FixedUpdate()
    {
        if (frameCnt != -1) frameCnt++;
    }

    // データの初期化（シーン管理でゲームを開始したと同時に呼び出され、使用パーツ情報が初期化される）
    public void Ready()
    {
        _sceneController = PlaySceneController.Instance;
        _data.ReadyPartsInfo(_sceneController.StageNum);
    }
    // リプレイの記録を開始する（ロボットの動き始めにフレームカウントを合わせたりできる）
    public void StartMemory()
    {
        frameCnt = 0;
        StartCoroutine(GetRobotTransformInterval());
    }
    // パーツの使用を記録（パーツ管理がパーツを使用した際に呼び出し）
    public void UseParts()
    {
        _data.RegisterUseParts(frameCnt);
    }
    // パーツの使用終了を記録（ロボットの状態管理が使用終了状態に遷移した際に呼び出し）
    public void EndUseParts()
    {
        _data.RegisterEndUseParts(frameCnt);
    }
    // パーツの獲得を記録（パーツ管理がパーツを獲得した際に呼び出し）
    public void GetParts(PartsInfo.PartsData data)
    {
        _data.RegisterGetParts(frameCnt, data.id, data.angle);
    }
    // 力を記録（力クラスが力を止めた時の自身、あるいはゲームが終了した時の残った力を記録）
    public void SetForce(IForce force)
    {
        _data.RegisterRobotForce(frameCnt, force);
    }
    // ゲーム結果を記録（ゲームが終了した際に記録）
    public void SetResult()
    {
        _data.RegisterResult(frameCnt, _sceneController.Score);
        frameCnt = -1;
    }
    // リプレイデータを保存する
    [ContextMenu("Debug/Save")]
    public void Save()
    {
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
            if(_sceneController.scene != PlaySceneController.E_PlayScene.GamePlay)
            {
                yield break;
            }
            robot.GetTransform(out Vector2 position, out Vector2 velocity);
            _data.RegisterRobotTransform(frameCnt, position, velocity);
            yield return waitForSeconds;
        }
    }
}
