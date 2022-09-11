using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 複数のシャドウを管理するComponent
public class ShadowManager : SingletonMonoBehaviourInSceneBase<ShadowManager>
{
    // シャドウロボットのPrefab
    [SerializeField] private ShadowRobot shadowPrefab;
    [SerializeField] private Vector2 firstPosition = new Vector2(0, -2.5f);

    // シーン上にあるシャドウロボットオブジェクトのリスト
    private bool IsStart = false;
    [SerializeField] private bool SelectReplayDataByIndex = false;
    public List<int> replayIndexList = new List<int>();
    private List<ShadowRobot> shadows = new List<ShadowRobot>();


    // 指定のリプレイのシャドウを追加する
    [ContextMenu("Debug/RegisterShadow")]
    public void RegisterShadow()
    {
        if (IsStart) return;
        if (SelectReplayDataByIndex)
        {
            // ReplayDatasのインデックスを指定してシャドウを生成
            foreach (var index in replayIndexList)
            {
                var shadow = Instantiate(shadowPrefab, firstPosition, Quaternion.identity);
                shadow.LoadReplayData(index);
                shadows.Add(shadow);
            }
        }
        else
        {
            // このステージのリプレイデータを全てシャドウにする
            foreach(var replayData in StageReplayDatas)
            {
                var shadow = Instantiate(shadowPrefab, firstPosition, Quaternion.identity);
                shadow.LoadReplayData(replayData);
                shadows.Add(shadow);
            }
        }
    }

    // シャドウのリプレイを開始する
    public void StartShadowReplay()
    {
        if (IsStart) return;
        IsStart = true;
        foreach(var shadow in shadows)
        {
            shadow.StartGame();
        }
    }

    // シャドウのリプレイが終了した際に呼び出される処理
    public void FinishReplay(ShadowRobot shadow)
    {
        // シャドウをリストから削除し、オブジェクトを破棄する
        shadows.Remove(shadow);
        if (shadow != null) Destroy(shadow.gameObject);
    }

    // 全てのシャドウを停止させる
    public void StopAllShadow()
    {
        for(int i = 0; i < shadows.Count; i++)
        {
            Destroy(shadows[i].gameObject);
        }
        shadows.Clear();
        IsStart = false;
    }


    // シャドウに使用するリプレイデータを選択する
    private List<ReplayData> StageReplayDatas => ReplayDatas.Instance.GetStageReplay(PlaySceneController.Instance.StageNum);
}
