using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// �j��\�ȃI�u�W�F�N�g�̓�����`����Component�B
[RequireComponent(typeof(Collider2D))]
public class CrashableObject : GimickBase
{
    // �M�~�b�N�U���^�O�̎w��
    static private string AttackerTag = "StageAttacker";

    public bool IsAlive { get; private set; } = true;
    private Collider2D _collider;

    [SerializeField] private float _destroyDuration = 0.1f;
    [Tooltip("�j���҂��Ԃɕ\�������X�v���C�g�B�ݒ肳��Ă��Ȃ���΁A�X�v���C�g��ύX���Ȃ��B")]
    [SerializeField] private Sprite _destroyAnimationSprite;
    [SerializeField] private PlaySceneController.E_HitStopType _hitEffectType = PlaySceneController.E_HitStopType.None;
    [SerializeField] private float _hitEffectTimeScale;
    [SerializeField] private float _hitEffectTime;
    [SerializeField] private UnityEvent _crashEvent = new UnityEvent();
    [SerializeField] private UnityEvent _resetEvent = new UnityEvent();

    // �p�[�W�X�v���C�g�̐ݒ�
    [Header("�p�[�W�ݒ�")]
    [SerializeField] private PurgeManager _purgeManager;
    [SerializeField] private List<Sprite> _purgeSprites = new List<Sprite>();

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    // �M�~�b�N�����Z�b�g���郁�\�b�h
    public override void ResetGimick() {
        gameObject.SetActive(true);
        _collider.enabled = true;
        IsAlive = true;
        _resetEvent.Invoke();
    }

    // �����ƏՓ˂����ہA�Ă΂�郁�\�b�h
    public void HitAttackCollider(Collider2D collision)
    {
        // �Փ˂����̂��U�����肾������A�j�󃁃\�b�h���ĂԁB
        if (!collision.CompareTag(AttackerTag)) return;
        Crash();
    }

    // �j�󎞂ɌĂяo����郁�\�b�h
    [ContextMenu("Debug/Crash")]
    private void Crash()
    {
        if (!IsAlive) return;
        IsAlive = false;

        // �����蔻��𖳌���
        _collider.enabled = false;
        if (gameObject.layer == LayerMask.NameToLayer("StageWall"))
        {
            // �n�ʂ̓����蔻��𖳌�������ۂ̓��{�b�g�̐ڒn��������Z�b�g����
            GimickManager.Instance.ResetOnGround();
        }

        // ���g��j������
        _crashEvent.Invoke();
        if (_destroyAnimationSprite != null && TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = _destroyAnimationSprite;
        }
        Invoke("DestroyAction", _destroyDuration);
    }

    // �j�󎞂̏���
    private void DestroyAction()
    {
        // �p�[�W�p�[�c�̐ݒ���s��
        if (_purgeManager)
        {
            _purgeManager.AddPartsBySprite(_purgeSprites);
        }
        gameObject.SetActive(false);
    }

    // �w�肳�ꂽ�ݒ�Ńq�b�g�X�g�b�v����������
    public void RequestHitStop()
    {
        if (_hitEffectType == PlaySceneController.E_HitStopType.Slow)
        {
            PlaySceneController.Instance.RequestHitStopBySlow(_hitEffectTimeScale, _hitEffectTime);
        }
        else if (_hitEffectType == PlaySceneController.E_HitStopType.Stop)
        {
            PlaySceneController.Instance.RequestHitStopByStop(_hitEffectTime);
        }
    }
}
