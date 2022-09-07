using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressData
{
    [SerializeField]
    private List<StageProgressData> stageProgressDatasList
        = new List<StageProgressData>();
    
    [SerializeField]
    private int clearStageNum;

    public ProgressData(List<Stage> stageList)
    {
        int stageNum = 0;
        foreach(Stage stage in stageList)
        {
            // 個々のステージの進行状況を作成
            StageProgressData progressData = stage.ProgressData;
            stageProgressDatasList.Add(progressData);

            // クリアしているステージ番号格納
            if (stage.ProgressData.IsClear) clearStageNum = stageNum;
        }
    }
}
