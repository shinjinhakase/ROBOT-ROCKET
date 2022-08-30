using System;
using UnityEngine;
using UnityEngine.Events;

// ���������I�u�W�F�N�g�ɕK���K�v��Component�B�������Ɏ��s���郁�\�b�h���C���X�y�N�^����w��ł���B
// �i�������Ɏ��g���A�N�e�B�u�ɐݒ肷��̂ŁA�s���̗ǂ��^�C�~���O�ŃA�N�e�B�u�ɂ���K�v������j
public class SummonableObject : MonoBehaviour
{
    [Tooltip("�������ꂽ�Ƃ��ɌĂяo�����֐��B�����̓p�[�c�f�[�^(PartsInfo.PartsData)�ƃ��{�b�g�̈ʒu(Transform)")]
    [SerializeField] public SummonActionEvent action;

    void Awake()
    {
        // �������Ɏ��g���A�N�e�B�u�ɂ��Ă���
        gameObject.SetActive(false);
    }

    // �������ꂽ�ۂɌĂяo����郁�\�b�h
    public void Summon(PartsInfo.PartsData data, Transform robotTransform)
    {
        action.Invoke(data, robotTransform);
    }

    [Serializable]
    public class SummonActionEvent : UnityEvent<PartsInfo.PartsData, Transform> { }
}
