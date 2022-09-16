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
    [Tooltip("SetPosition���Ăяo���ꂽ�ۂ̐ݒ菉���ʒu")]
    [SerializeField] private Vector3 _localPosition = Vector3.zero;
    [SerializeField] private float _explodeDistance = 0f;

    [Header("Rigidbody2D�֘A")]
    [Tooltip("Rigidbody2D���A�^�b�`����Ă����ہA�������ɉ����鏉���ݒ�")]
    [SerializeField] private Vector2 initVelocity = Vector2.zero;
    private bool _haveRigidbody2D;
    public bool HaveRigidbody2D { get { return _haveRigidbody2D; } private set { _haveRigidbody2D = value; } }
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

        // �X�e�[�W���Z�b�g���̍폜�Ώۂɓo�^����
        GimickManager.Instance.RegisterAsDeleteObject(gameObject);
    }
    // �������ꂽ�ۂɌĂяo����郁�\�b�h�i�������ݒ肷��j
    public void Summon(PartsInfo.PartsData data, Transform robotTransform, Vector2 initVelocity)
    {
        if (IsActiveWhenSummon) gameObject.SetActive(true);
        action.Invoke(data, robotTransform);
        if (HaveRigidbody2D) _rb.velocity = initVelocity;   // Ridigbody2D���A�^�b�`����Ă���ΐݒ肳�ꂽ������������
        afterAction.Invoke();

        // �ݒ肪����Ă���΁A�w��b����Ɏ��g��j������
        if (IsDestroyAfterSeconds) Destroy(gameObject, _destroyDuration);

        // �X�e�[�W���Z�b�g���̍폜�Ώۂɓo�^����
        GimickManager.Instance.RegisterAsDeleteObject(gameObject);
    }

    // �ʒu��ݒ肷��ėp���\�b�h
    public void SetPosition(PartsInfo.PartsData _, Transform target)
    {
        transform.position = target.position + _localPosition;
    }
    // �e�������҂ɐݒ肷��߂�����
    public void SetParentToSummoner(PartsInfo.PartsData _, Transform summoner)
    {
        transform.SetParent(summoner);
    }
    // �ʒu�𔚒e�̈ʒu�ɐݒ肷�郁�\�b�h
    public void SetPositionToBombLocate(PartsInfo.PartsData data, Transform summoner)
    {
        transform.position = summoner.position + Quaternion.Euler(0, 0, data.angle - 180) * Vector3.right * _explodeDistance;
    }

    // �p�[�c�̎g�p�I�����Ԃ�j�����Ԃɐݒ肷��
    public void SetDestroyTimeToPartsDestroy(PartsInfo.PartsData data, Transform summoner)
    {
        if (data == null) return;
        var performance = PlayPartsManager.Instance.GetPerformance(data.id);
        if (performance.forceType == PartsPerformance.E_ForceType.Bomb) Destroy(gameObject);
        else if (performance.forceType == PartsPerformance.E_ForceType.NoForce) Destroy(gameObject);
        else Destroy(gameObject, performance.t);
    }

    [Serializable]
    public class SummonActionEvent : UnityEvent<PartsInfo.PartsData, Transform> { }
}
