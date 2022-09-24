using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimickManager : SingletonMonoBehaviourInSceneBase<GimickManager>
{
    private List<GimickBase> gimickList = new List<GimickBase>();
    private List<GameObject> deleteObjects = new List<GameObject>();    // �X�e�[�W���Z�b�g���ɍ폜����I�u�W�F�N�g�̃��X�g

    [SerializeField] private List<SoundBeep> soundBeeps = new List<SoundBeep>();

    [SerializeField] private MainRobot _mainRobot;

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

    // �u���b�N�j��̉���炷
    public void BeepSEByIndex(int index)
    {
        if (index < 0 || index >= gimickList.Count) {
            Debug.LogWarning("���ʉ��̃C���f�b�N�X�w�肪�s���ł��B");
            return;
        }
        soundBeeps[index].Beep();
    }

    // �n�ʂ̓����蔻�肪Destroy���ŏ������ۂ�OnTriggerExit���������Ȃ��̂ŁAAnimator��OnGround�i�ڒn����j�����Z�b�g����B
    public void ResetOnGround()
    {
        _mainRobot._status.SetOnGround(false);
        ShadowManager.Instance.ResetOnGround();
    }
}
