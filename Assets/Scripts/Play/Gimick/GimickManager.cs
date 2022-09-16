using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimickManager : SingletonMonoBehaviourInSceneBase<GimickManager>
{
    private List<GimickBase> gimickList = new List<GimickBase>();
    private List<GameObject> deleteObjects = new List<GameObject>();    // �X�e�[�W���Z�b�g���ɍ폜����I�u�W�F�N�g�̃��X�g

    // �M�~�b�N���Ǘ����X�g�ɒǉ�����
    public void SaveGimick(GimickBase gimick)
    {
        gimickList.Add(gimick);
    }

    // �o�^���Ă��铮�I�ȃX�e�[�W�M�~�b�N������������
    public void ResetGimick()
    {
        // �M�~�b�N�����Z�b�g
        foreach(var gimick in gimickList)
        {
            gimick.ResetGimick();
        }
        
        // �폜�ΏۃI�u�W�F�N�g���폜
        foreach(var deleteObject in deleteObjects)
        {
            if (deleteObject) Destroy(deleteObject);
        }
        deleteObjects.Clear();
    }

    // ���{�b�g�̓����n�߂Ɠ����ɃX�e�[�W�M�~�b�N�̏������J�n����
    public void StartGimick()
    {
        foreach(var gimick in gimickList)
        {
            gimick.OnStartRobot();
        }
    }


    // ���Z�b�g���̍폜�ΏۃI�u�W�F�N�g�ɓo�^
    public void RegisterAsDeleteObject(GameObject target)
    {
        deleteObjects.Add(target);
    }
}
