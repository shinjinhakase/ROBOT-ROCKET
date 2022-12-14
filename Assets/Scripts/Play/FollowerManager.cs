using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    [SerializeField] private FollowIcon followPrefab;
    [SerializeField] private ReplayPlayer replayPlayer;

    private List<FollowIcon> followers = new List<FollowIcon>();

    // 追従アイコンをID指定で初期化する（ReplayPlayer用）
    public void InitFunctionForReplayPlayer()
    {
        InitFollowersByIDList(replayPlayer.ReadyPartsList);
    }
    private void InitFollowersByIDList(List<PartsInfo.PartsData> datas)
    {
        DestroyAllFollowers();
        PlayPartsManager _playPartsManager = PlayPartsManager.Instance;
        foreach (var data in datas)
        {
            var follower = Instantiate(followPrefab, transform.position, Quaternion.identity, transform);
            follower.SetSprite(_playPartsManager.GetPerformance(data.id).iconSprite, data);
            if (followers.Count == 0) follower.target = transform;
            else follower.target = followers[^1].transform;
            followers.Add(follower);
        }
    }

    // 追従アイコンをPartsInfoのリストから初期化する
    public void InitFollowerByPartsInfo()
    {
        DestroyAllFollowers();
        PlayPartsManager _playPartsManager = PlayPartsManager.Instance;
        foreach (var data in PartsInfo.Instance.GetPartsList())
        {
            var follower = Instantiate(followPrefab, transform.position, Quaternion.identity, transform);
            follower.SetSprite(_playPartsManager.GetPerformance(data.id).iconSprite, data);
            if (followers.Count == 0) follower.target = transform;
            else follower.target = followers[^1].transform;
            followers.Add(follower);
        }
    }

    // 使用したパーツのアイコンを消す処理
    public void UseParts()
    {
        if (followers.Count == 0) return;
        if (followers.Count >= 2)
        {
            followers[1].target = followers[0].target;
        }
        followers[0].DestroyMyself();
        followers.RemoveAt(0);
    }

    // 獲得したパーツのアイコンを増やす処理
    public void GetParts(PartsInfo.PartsData data)
    {
        var follower = Instantiate(followPrefab, transform.position, Quaternion.identity, transform);
        follower.SetSprite(PlayPartsManager.Instance.GetPerformance(data.id).iconSprite, data);
        if (followers.Count == 0) follower.target = transform;
        else follower.target = followers[^1].transform;
        followers.Add(follower);
    }

    // アイコンを全て削除する
    public void DestroyAllFollowers()
    {
        for(int i = 0; i < followers.Count; i++)
        {
            followers[i].DestroyMyself();
        }
        followers.Clear();
    }
}
