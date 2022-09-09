using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimickManager : SingletonMonoBehaviourInSceneBase<GimickManager>
{
    private List<GimickBase> gimickList = new List<GimickBase>();

    // �M�~�b�N���Ǘ����X�g�ɒǉ�����
    public void SaveGimick(GimickBase gimick)
    {
        gimickList.Add(gimick);
    }

    // �o�^���Ă��铮�I�ȃX�e�[�W�M�~�b�N������������
    public void ResetGimick()
    {
        foreach(var gimick in gimickList)
        {
            gimick.ResetGimick();
        }
    }

    // ���{�b�g�̓����n�߂Ɠ����ɃX�e�[�W�M�~�b�N�̏������J�n����
    public void StartGimick()
    {
        foreach(var gimick in gimickList)
        {
            gimick.OnStartRobot();
        }
    }
}
