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
    [SerializeField] private UnityEvent _crashEvent = new UnityEvent();

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
        Destroy(gameObject);
    }
}
