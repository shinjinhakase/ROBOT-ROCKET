using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressData : SavableSingletonBase<ProgressData>
{
    [SerializeField]
    private List<StageProgressData> stageProgressDatasList
        = new List<StageProgressData>();
    
    [SerializeField]
    private int clearStageNum;

    public List<StageProgressData> StageProgressDataList
    {
        get { return stageProgressDatasList; }
    }

    public int ClearStageNum
    {
        get { return clearStageNum; }
    }

    public void SetStageList(List<Stage> stageList)
    {
        int stageNum = 0;
        foreach(Stage stage in stageList)
        {
            // �X�̃X�e�[�W�̐i�s�󋵂��쐬
            StageProgressData progressData = stage.ProgressData;
            stageProgressDatasList.Add(progressData);

            // �N���A���Ă���X�e�[�W�ԍ��i�[
            if (stage.ProgressData.IsClear) clearStageNum = stageNum;
        }
    }
}
