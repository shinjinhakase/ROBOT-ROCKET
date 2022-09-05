using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 複数のシャドウを管理するComponent
public class ShadowManager : SingletonMonoBehaviourInSceneBase<ShadowManager>
{
    // シャドウロボットのPrefab
    [SerializeField] private ShadowRobot shadowPrefab;

    // シーン上にあるシャドウロボットオブジェクトのリスト
    private bool IsStart = false;
    public List<int> replayIndexList = new List<int>();
    private List<ShadowRobot> shadows = new List<ShadowRobot>();


    // 指定のリプレイのシャドウを追加する
    [ContextMenu("Debug/RegisterShadow")]
    public void RegisterShadow()
    {
        if (IsStart) return;
        foreach(var index in replayIndexList)
        {
            var shadow = Instantiate(shadowPrefab);
            shadow.LoadReplayData(index);
            shadows.Add(shadow);
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
        if (shadow != null) Destroy(shadow);
    }

    // 全てのシャドウを停止させる
    public void StopAllShadow()
    {
        for(int i = 0; i < shadows.Count; i++)
        {
            Destroy(shadows[i]);
        }
        shadows.Clear();
        IsStart = false;
    }
}
