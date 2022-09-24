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
    private List<ReplayData> _useReplayDatas = new List<ReplayData>();
    private List<ShadowRobot> shadows = new List<ShadowRobot>();


    // 指定のリプレイのシャドウを追加する
    [ContextMenu("Debug/RegisterShadow")]
    public void RegisterShadow()
    {
        if (IsStart) return;

        // リプレイモードでは無いとき、シャドウに用いるリプレイデータを更新する
        if (!PlaySceneController.Instance.IsReplayMode)
        {
            // このステージのリプレイデータを全てシャドウにする
            _useReplayDatas = new List<ReplayData>(StageReplayDatas);
        }

        // このステージのリプレイデータを全てシャドウにする
        foreach (var replayData in _useReplayDatas)
        {
            var shadow = Instantiate(shadowPrefab, firstPosition, Quaternion.identity);
            shadow.LoadReplayData(replayData);
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


    public void ResetOnGround()
    {
        foreach(var shadow in shadows)
        {
            shadow?.ResetOnGround();
        }
    }

    // シャドウに使用するリプレイデータを選択する
    private List<ReplayData> StageReplayDatas => ReplayDatas.Instance.GetStageReplay(PlaySceneController.Instance.StageNum);
}
