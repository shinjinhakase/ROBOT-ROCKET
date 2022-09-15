using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ���{�b�g�̏�Ԃ��Ǘ�����N���X�B�A�j���[�V�����Ȃǂ����������肷��B
// ����L�����E���v���C�E�V���h�E�̊�ՁB
[RequireComponent(typeof(PurgeManager))]
public class RobotStatus : MonoBehaviour
{
    // ���{�b�g�̏�Ԃ������񋓌^
    private enum E_RobotStatus
    {
        Ready,      // �Q�[���J�n�O�̑ҋ@���
        Fly,        // ��s��ԁi�ʏ�̑ҋ@��ԁj
        UseParts,   // �A�C�e���g�p��
        Cooldown,   // �N�[���^�C���ҋ@��
        EndFly,     // ��s�I���i�Q�[���N���A��Q�[���I�[�o�[�Ȃǁj
    }
    private E_RobotStatus _status = E_RobotStatus.Ready;
    private int cooltime = 0;       // �N�[���^�C��

    private Sprite usingPartsSprite = null;

    // ���{�b�g�̏�Ԕ��胁�\�b�h
    public bool IsWaitingForFly => _status == E_RobotStatus.Ready;  // ��s���J�n����
    public bool IsPartsUsable => _status == E_RobotStatus.Fly;  // �����p�[�c�̎g�p�\����
    public bool IsUsingParts => _status == E_RobotStatus.UseParts;  // �p�[�c�̎g�p������
    public bool IsFlying => _status != E_RobotStatus.Ready && _status != E_RobotStatus.EndFly;  // ��s������i�Q�[��������j
    public bool IsEndFly => _status == E_RobotStatus.EndFly;    // ��s�I������


    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Animator _animator;
    private PurgeManager _purgeManager;

    [SerializeField] private List<Sprite> GameOverRobotPurgeData = new List<Sprite>();

    [Header("�C�x���g�n��")]
    [Tooltip("�p�[�c�̎g�p�J�n���ɌĂ΂�郁�\�b�h")]
    [SerializeField] private UnityEvent startUsePartsEvent = new UnityEvent();
    [Tooltip("�p�[�c�̎g�p�I�����ɌĂ΂�郁�\�b�h")]
    [SerializeField] private UnityEvent endUsePartsEvent = new UnityEvent();

    private void Awake()
    {
        _purgeManager = GetComponent<PurgeManager>();
    }

    private void FixedUpdate()
    {
        // �N�[���^�C������i�������@�̓R���[�`����WaitForSeconds�Ƃ��g���ׂ��������Ă�j
        if (_status == E_RobotStatus.Cooldown && cooltime > 0)
        {
            cooltime--;
            if (cooltime <= 0)
            {
                endCooltime();
            }
        }
    }

    // �Q�[���J�n���ɌĂԃ��\�b�h
    public void startGame()
    {
        if(_status != E_RobotStatus.Ready)
        {
            Debug.LogWarning("�Q�[���J�n�O�ȊO�ɃQ�[���J�n���\�b�h���Ă΂�܂����B");
            return;
        }

        _status = E_RobotStatus.Fly;

        // ��s�̃A�j���[�V�����֑J�ځiOnGround�ŏ���ɑJ�ڂ���j
    }

    // �p�[�c�̎g�p�J�n
    public void startUseParts(PartsPerformance performance, PartsInfo.PartsData data)
    {
        // �A�C�e�����g�p�ł����Ԃ�����
        if (!IsPartsUsable)
        {
            Debug.LogWarning("�p�[�c�̎g�p��ԂɈڂ�܂���ł����B");
            return;
        }

        startUsePartsEvent.Invoke();

        // �A�C�e���g�p��Ԃ֏�ԑJ��
        _status = E_RobotStatus.UseParts;

        // �p�[�W����ۂɓ����o���p�[�c�̌�����
        usingPartsSprite = performance.partsSprite;

        // �N�[���^�C�����v�Z���Ă���
        cooltime = Mathf.RoundToInt(performance.cooltime / Time.fixedDeltaTime);

        // �A�C�e���̎�ނɂ���ē��L�̃A�j���[�V�����֑J��
        if (performance.forceType == PartsPerformance.E_ForceType.Rocket)
        {
            _animator.SetTrigger("Rocket");
        }
        else if (performance.forceType == PartsPerformance.E_ForceType.Propeller)
        {
            _animator.SetTrigger("Propeller");
        }
        else if (performance.forceType == PartsPerformance.E_ForceType.Glider)
        {
            _animator.SetTrigger("Glider");
        }
    }

    // �p�[�c�̌��ʏI��
    public void endUseParts()
    {
        if (!IsUsingParts)
        {
            Debug.LogWarning("�p�[�c�̎g�p���ȊO�Ƀp�[�c�g�p�I�����\�b�h���Ăяo����܂����B");
            return;
        }

        // ��ԑJ�ځi�N�[���^�C��������Αҋ@��Ԃֈڍs����j
        if (cooltime > 0)
        {
            _status = E_RobotStatus.Cooldown;
            _animator.SetBool("Cooltime", true);
        }
        else _status = E_RobotStatus.Fly;

        // �g�p���I������p�[�c���p�[�W���ē����o��
        if (usingPartsSprite != null)
        {
            _purgeManager.AddPartsBySprite(usingPartsSprite);
            usingPartsSprite = null;
        }

        endUsePartsEvent.Invoke();
        // ��sor�N�[���^�C���̃A�j���[�V�����ɑJ�ڂ���
        _animator.SetTrigger("EndUse");
    }

    // �N�[���^�C���̏I��
    public void endCooltime()
    {
        if (!IsFlying)
        {
            Debug.LogWarning("��s���ȊO�ɃN�[���_�E���I�����\�b�h���Ă΂�܂����B");
            return;
        }

        _status = E_RobotStatus.Fly;
        cooltime = 0;

        // ��s�A�j���[�V�����ɑJ�ڂ���
        _animator.SetBool("Cooltime", false);
    }

    // �Q�[���N���A���ɌĂяo����郁�\�b�h
    public void GameClear()
    {
        if (!IsFlying)
        {
            Debug.LogWarning("�N���A���\�b�h����s���ȊO�ɌĂяo����܂����B");
            return;
        }
        _status = E_RobotStatus.EndFly;

        // �N���A���̃A�j���[�V�����Ȃǂ̃��{�b�g�֌W�̏���
        _animator.SetTrigger("Clear");
    }

    // �Q�[�����s���ɌĂяo����郁�\�b�h
    public void GameOver()
    {
        if (!IsFlying)
        {
            Debug.LogWarning("�Q�[���I�[�o�[���\�b�h����s���ȊO�ɌĂяo����܂����B");
            return;
        }
        _status = E_RobotStatus.EndFly;

        // TODO�F�Q�[�����s���̃A�j���[�V�����Ȃǂ̃��{�b�g�֌W�̏���
        _purgeManager.AddPartsBySprite(GameOverRobotPurgeData);
    }

    // �J�X�^�����j���[���J�����ۂɌĂяo����郁�\�b�h
    public void OpenCustomMenu()
    {
        if (PlaySceneController.Instance.IsOpenableCustomMenu)
        {
            bodyCollider.enabled = false;
            _status = E_RobotStatus.EndFly;
        }
    }

    // ���߂����蒼���ۂɌĂяo����郁�\�b�h
    public void ResetStatus()
    {
        // �A�j���[�^�[�̏�Ԃ����Z�b�g����
        _animator.SetBool("Cooltime", false);
        _animator.SetBool("OnGround", true);
        _animator.Play("robot");    // robot�X�e�[�g�ɐ؂�ւ���

        bodyCollider.enabled = true;
        _status = E_RobotStatus.Ready;
        cooltime = 0;
    }


    // �A�j���[�^�[�ɒn�ʂɐڂ��Ă��邩��`����
    public void SetOnGround(bool IsGround)
    {
        _animator.SetBool("OnGround", IsGround);
    }
}
