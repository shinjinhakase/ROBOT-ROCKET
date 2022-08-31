using System.Collections;
using UnityEngine;

// �j��\�I�u�W�F�N�g(CrashableObject)��j��\�ȃI�u�W�F�N�g���`����Component�B
public class CrasherObject : MonoBehaviour
{
    private enum E_DisableSetting
    {
        NoSetting,
        DisableWhenStartAttack,
        DisableWhenEndAttack
    }

    [Tooltip("�U���J�n�̓����蔻���S��Collider2D���Z�b�g����")]
    [SerializeField] private Collider2D CollisionDetectCollider;

    [Tooltip("�U���̓����蔻���S��Collider2D���Z�b�g����")]
    [SerializeField] private Collider2D AttackRangeCollider;

    [Tooltip("�U���̓����蔻�肪�o������b��")]
    [SerializeField] private float AttackTime = 1f;

    [Tooltip("�U�����I�����Ă���I�u�W�F�N�g���j�������܂ł̃^�C�����O")]
    [SerializeField] private float DestroyDuration = 0f;

    // �����E���蓮��Ɋւ��ݒ�
    [Header("�ڍאݒ�")]
    [Tooltip("����Rigidbody2D���A�^�b�`����Ă���Ȃ�A�w��̃^�C�~���O�ňړ��E��]���Œ肵�܂�")]
    [SerializeField] private E_DisableSetting DisableRidigbody2DSetting = E_DisableSetting.NoSetting;
    [Tooltip("��������ɗp���Ă��Ȃ�Collider2D���A�^�b�`����Ă���Ȃ�A�w��̃^�C�~���O�Ŗ��������܂�")]
    [SerializeField] private E_DisableSetting DisableCollider2DSetting = E_DisableSetting.NoSetting;


    public bool IsStartAttack { get; private set; } = false;

    private IEnumerator timerCoroutine;
    private Rigidbody2D rb = null;
    private Collider2D _collider = null;

    // Start is called before the first frame update
    private void Awake()
    {
        // �����蔻��̗L�������m�F���Ă���
        CollisionDetectCollider.enabled = true;
        AttackRangeCollider.enabled = false;

        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    [ContextMenu("Debug/StartAttack")]
    private void StartAttackForDebug()
    {
        StartAttack(CollisionDetectCollider);
    }

    // �U���^�C�}�[���X�^�[�g������
    public void StartTimer(float time)
    {
        if (timerCoroutine != null || IsStartAttack) return;
        timerCoroutine = SetTimerCoroutine(time);
        StartCoroutine(timerCoroutine);
    }

    // �U���J�n���ɓ����蔻��Collider����Ăяo�����A�Փ˃��\�b�h
    public void StartAttack(Collider2D other)
    {
        if (IsStartAttack) return;
        IsStartAttack = true;

        // �^�C�}�[���ݒ肳��Ă����ꍇ�͐؂�
        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        // Ridigbody2D�̈ړ���ݒ莟��Ŗ���������
        if(rb != null && DisableRidigbody2DSetting == E_DisableSetting.DisableWhenStartAttack)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        // Collider2D��ݒ莟��Ŗ���������
        if (_collider != null && DisableCollider2DSetting == E_DisableSetting.DisableWhenStartAttack)
        {
            _collider.enabled = false;
        }

        // �Փ˔�����I�t�ɂ��A�U���̓����蔻����I���ɂ���
        CollisionDetectCollider.enabled = false;
        AttackRangeCollider.enabled = true;

        // �Փˈȍ~�̏������n�߂�
        StartCoroutine(ControlAttack());
    }

    // �w��b����ɍU���J�n���\�b�h���Ăяo�����\�b�h
    private IEnumerator SetTimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        StartAttack(null);
    }

    // �U�����J�n���Ă���̏����Ǘ����\�b�h
    private IEnumerator ControlAttack()
    {
        yield return new WaitForSeconds(AttackTime);
        AttackRangeCollider.enabled = false;
        // Ridigbody2D�̈ړ���ݒ莟��Ŗ���������
        if (rb != null && DisableRidigbody2DSetting == E_DisableSetting.DisableWhenEndAttack)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        // Collider2D��ݒ莟��Ŗ���������
        if (_collider != null && DisableCollider2DSetting == E_DisableSetting.DisableWhenEndAttack)
        {
            _collider.enabled = false;
        }
        Destroy(gameObject, DestroyDuration);
    }
}
