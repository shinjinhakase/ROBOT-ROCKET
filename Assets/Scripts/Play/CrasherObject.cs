using System.Collections;
using UnityEngine;

// �j��\�I�u�W�F�N�g(CrashableObject)��j��\�ȃI�u�W�F�N�g���`����Component�B
public class CrasherObject : MonoBehaviour
{
    [Tooltip("�U���J�n�̓����蔻���S��Collider2D���Z�b�g����")]
    [SerializeField] private Collider2D CollisionDetectCollider;

    [Tooltip("�U���̓����蔻���S��Collider2D���Z�b�g����")]
    [SerializeField] private Collider2D AttackRangeCollider;

    [Tooltip("�U���̓����蔻�肪�o������b��")]
    [SerializeField] private float AttackTime = 1f;

    [Tooltip("�U�����I�����Ă���I�u�W�F�N�g���j�������܂ł̃^�C�����O")]
    [SerializeField] private float DestroyDuration = 0f;

    public bool IsStartAttack { get; private set; } = false;

    // Start is called before the first frame update
    private void Awake()
    {
        // �����蔻��̗L�������m�F���Ă���
        CollisionDetectCollider.enabled = true;
        AttackRangeCollider.enabled = false;
    }

    [ContextMenu("Debug/StartAttack")]
    private void StartAttackForDebug()
    {
        StartAttack(CollisionDetectCollider);
    }

    // �U���J�n���ɓ����蔻��Collider����Ăяo�����A�Փ˃��\�b�h
    public void StartAttack(Collider2D other)
    {
        if (IsStartAttack) return;
        IsStartAttack = true;

        // �Փ˔�����I�t�ɂ��A�U���̓����蔻����I���ɂ���
        CollisionDetectCollider.enabled = false;
        AttackRangeCollider.enabled = true;

        // �Փˈȍ~�̏������n�߂�
        StartCoroutine(ControlAttack());
    }

    // �U�����J�n���Ă���̏����Ǘ����\�b�h
    private IEnumerator ControlAttack()
    {
        yield return new WaitForSeconds(AttackTime);
        AttackRangeCollider.enabled = false;
        Destroy(gameObject, DestroyDuration);
    }
}
