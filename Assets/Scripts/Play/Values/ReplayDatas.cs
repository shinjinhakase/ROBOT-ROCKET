using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���v���C�f�[�^���܂Ƃ߂ĕۊǂ���V���O���g��
public class ReplayDatas : SavableSingletonBase<ReplayDatas>
{
    // ���v���C�f�[�^�̃��X�g
    public List<ReplayData> datas = new List<ReplayData>();
    public List<ReplayData> GetDatas() => datas;
    public int Length => datas.Count;
    public ReplayData GetData(int index) => datas[index];

    // ���v���C�f�[�^��ǉ�����
    public void RegisterData(ReplayData data)
    {
        // �Â����̂������A���v���C�ő吔��ݒ肷��
        List<ReplayData> nowDataList = GetStageReplay(data.StageNum);
        if (nowDataList?.Count > 4) datas.Remove(nowDataList[0]);

        datas.Add(data);
    }
    // �w��̃X�e�[�W�̃��v���C�f�[�^���擾����
    public List<ReplayData> GetStageReplay(int StageNum)
    {
        return datas.FindAll(data => data.StageNum == StageNum);
    }
    // �w��̃X�e�[�W�̃n�C�X�R�A�̃��v���C�f�[�^���擾����
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
