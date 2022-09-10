using System;
using UnityEngine;
using UnityEngine.Events;

// ���������I�u�W�F�N�g�ɕK���K�v��Component�B�������Ɏ��s���郁�\�b�h���C���X�y�N�^����w��ł���B
// �i�������Ɏ��g���A�N�e�B�u�ɐݒ肷��̂ŁA�s���̗ǂ��^�C�~���O�ŃA�N�e�B�u�ɂ���K�v������j
public class SummonableObject : MonoBehaviour
{
    [Header("�C�x���g�֘A")]
    [Tooltip("�������ꂽ�Ƃ��ɏ����ݒ�̂��ߌĂяo�����֐��B�����̓p�[�c�f�[�^(PartsInfo.PartsData)�Ə����҂̈ʒu(Transform)")]
    [SerializeField] public SummonActionEvent action;
    [Tooltip("��������action�����s���ꂽ��Ɏ��s�����֐��B�����Ȃ��B")]
    [SerializeField] public UnityEvent afterAction;

    [Header("�������ݒ�")]
    [Tooltip("true�ɂ���ƁA�������Ɏ��g��Active�ɂ���")]
    [SerializeField] private bool IsActiveWhenSummon = true;
    [Tooltip("true�ɂ���ƁA���Ҍ�Ɏw��b���o�Ǝ����ŏ��ł���")]
    [SerializeField] private bool IsDestroyAfterSeconds = false;
    [Tooltip("IsDestroyAfterSeconds��true�̍ۂ́A���ŕb���ݒ�")]
    [SerializeField] private float _destroyDuration = 0f;

    [Header("Rigidbody2D�֘A")]
    [Tooltip("Rigidbody2D���A�^�b�`����Ă����ہA�������ɉ����鏉���ݒ�")]
    [SerializeField] private Vector2 initVelocity = Vector2.zero;
    private bool HaveRigidbody2D;
    private Rigidbody2D _rb;

    void Awake()
    {
        // �������Ɏ��g���A�N�e�B�u�ɂ��Ă���
        gameObject.SetActive(false);

        HaveRigidbody2D = TryGetComponent(out _rb);
    }

    // �������ꂽ�ۂɌĂяo����郁�\�b�h
    public void Summon(PartsInfo.PartsData data, Transform robotTransform)
    {
        if (IsActiveWhenSummon) gameObject.SetActive(true);
        action.Invoke(data, robotTransform);
        if (HaveRigidbody2D) _rb.velocity = initVelocity;   // Ridigbody2D���A�^�b�`����Ă���ΐݒ肳�ꂽ������������
        afterAction.Invoke();

        // �ݒ肪����Ă���΁A�w��b����Ɏ��g��j������
        if (IsDestroyAfterSeconds) Destroy(gameObject, _destroyDuration);
    }

    [Serializable]
    public class SummonActionEvent : UnityEvent<PartsInfo.PartsData, Transform> { }
}
