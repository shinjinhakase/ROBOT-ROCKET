using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����I�ɃI���E�I�t��؂�ւ���M�~�b�N
public class IntervalFlickerGimick : IntervalActionGimick
{
    [Tooltip("�I���E�I�t��؂�ւ���ΏہB���g��I�������A�q�I�u�W�F�N�g�Ȃǂ�I�Ԃ��ƁB")]
    [SerializeField] private GameObject target;
    [Tooltip("�Q�[�����J�n�����ۂ̏������")]
    [SerializeField] private bool InitialState;

    // �V�[�����J�n�����ۂɌĂ΂�郁�\�b�h
    public override void OnSceneStart() {
        target.SetActive(InitialState);
    }

    // �M�~�b�N�����Z�b�g���郁�\�b�h
    public override void ResetGimick()
    {
        base.ResetGimick();
        target.SetActive(InitialState);
    }

    // ����I�ɌĂяo�����A�N�V�������\�b�h
    protected override void Action()
    {
        base.Action();
        target.SetActive(!target.activeSelf);
    }

}
