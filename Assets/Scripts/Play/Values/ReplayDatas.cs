using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// リプレイデータをまとめて保管するシングルトン
public class ReplayDatas : SavableSingletonBase<ReplayDatas>
{
    // リプレイデータのリスト
    public List<ReplayData> datas = new List<ReplayData>();
    public List<ReplayData> GetDatas() => datas;
    public int Length => datas.Count;
    public ReplayData GetData(int index) => datas[index];

    // リプレイデータを追加する
    public void RegisterData(ReplayData data)
    {
        // 古いものを消し、リプレイ最大数を設定する
        List<ReplayData> nowDataList = GetStageReplay(data.StageNum);
        if (nowDataList?.Count > 4) datas.Remove(nowDataList[0]);

        datas.Add(data);
    }
    // 指定のステージのリプレイデータを取得する
    public List<ReplayData> GetStageReplay(int StageNum)
    {
        return datas.FindAll(data => data.StageNum == StageNum);
    }
    // 指定のステージのハイスコアのリプレイデータを取得する
    public ReplayData GetHighScoreReplay(int StageNum)
    {
        var dataList = GetStageReplay(StageNum);
        ReplayData highscore = dataList[0];
        for(int i = 1; i < dataList.Count; i++)
        {
            if (highscore.score <= dataList[i].score)
            {
                highscore = dataList[i];
            }
        }
        return highscore;
    }
}
