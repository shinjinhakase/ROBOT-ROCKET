using UnityEngine;

// �j��\�ȃI�u�W�F�N�g�̓�����`����Component�B
[RequireComponent(typeof(Collider2D))]
public class CrashableObject : GimickBase
{
    // �M�~�b�N�U���^�O�̎w��
    static private string AttackerTag = "StageAttacker";

    public bool IsAlive { get; private set; } = true;
    private Collider2D _collider;

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
        gameObject.SetActive(false);
    }
}
