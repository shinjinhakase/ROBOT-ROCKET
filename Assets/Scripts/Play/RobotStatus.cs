using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���{�b�g�̏�Ԃ��Ǘ�����N���X�B�A�j���[�V�����Ȃǂ����������肷��B
// ����L�����E���v���C�E�V���h�E�̊�ՁB
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

    // ���{�b�g�̏�Ԕ��胁�\�b�h
    public bool IsPartsUsable => _status == E_RobotStatus.Fly;  // �����p�[�c�̎g�p�\����
    public bool IsUsingParts => _status == E_RobotStatus.UseParts;  // �p�[�c�̎g�p������
    public bool IsFlying => _status != E_RobotStatus.Ready && _status != E_RobotStatus.EndFly;  // ��s������i�Q�[��������j
    public bool IsEndFly => _status == E_RobotStatus.EndFly;    // ��s�I������


    [SerializeField] private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �N�[���^�C������
        if (IsFlying && cooltime > 0)
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

        // TODO�F��s�̃A�j���[�V�����֑J��
    }

    // �p�[�c�̎g�p�J�n
    public void startUseParts(PartsInfo.PartsData data)
    {
        // �A�C�e�����g�p�ł����Ԃ�����
        if (!IsPartsUsable)
        {
            Debug.LogWarning("�p�[�c�̎g�p��ԂɈڂ�܂���ł����B");
            return;
        }

        // ��ԑJ��
        _status = E_RobotStatus.UseParts;

        // TODO�F�A�C�e���̎�ނɂ���ē��L�̃A�j���[�V�����֑J��
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
        if (cooltime <= 0) _status = E_RobotStatus.Cooldown;
        else _status = E_RobotStatus.Fly;

        // TODO�F��sor�N�[���^�C���̃A�j���[�V�����ɑJ�ڂ���
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

        // TODO�F��s�A�j���[�V�����ɑJ�ڂ���
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

        // TODO�F�N���A���̃A�j���[�V�����Ȃǂ̃��{�b�g�֌W�̏���
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
    }
}
