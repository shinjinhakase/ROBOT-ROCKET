using UnityEngine;

// �j��\�ȃI�u�W�F�N�g�̓�����`����Component�B
[RequireComponent(typeof(Collider2D))]
public class CrashableObject : MonoBehaviour
{
    // �M�~�b�N�U���^�O�̎w��
    static private string AttackerTag = "StageAttacker";

    [Tooltip("�j�󂳂�Ă��犮�S�ɃI�u�W�F�N�g��������܂ł̎���")]
    [SerializeField] private float CrashDuration = 0f;

    public bool IsAlive { get; private set; } = true;
    private Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
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

        // �w��b����Ɏ��g��j�󂷂�i�A�j���[�V�����p�̑ҋ@���ԁj
        Destroy(gameObject, CrashDuration);
    }
}
