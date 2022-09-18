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

    private Sprite usingPartsSprite = null; // �������Ă���g�p���p�[�W�X�v���C�g�̎Q��

    // ���{�b�g�̏�Ԕ��胁�\�b�h
    public bool IsWaitingForFly => _status == E_RobotStatus.Ready;  // ��s���J�n����
    public bool IsPartsUsable => _status == E_RobotStatus.Fly;  // �����p�[�c�̎g�p�\����
    public bool IsUsingParts => _status == E_RobotStatus.UseParts;  // �p�[�c�̎g�p������
    public bool IsFlying => _status != E_RobotStatus.Ready && _status != E_RobotStatus.EndFly;  // ��s������i�Q�[��������j
    public bool IsEndFly => _status == E_RobotStatus.EndFly;    // ��s�I������


    // �L���b�V����
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _usePartsAudioSource;
    [SerializeField] private AudioSource _purgePartsAudioSource;
    private AudioClip _purgePartsSE;
    private PurgeManager _purgeManager;

    [SerializeField] private ParticleSystem _usePartsEffect = null;

    // �Q�[���I�[�o�[���̃p�[�W���{�b�g�X�v���C�g�f�[�^
    [SerializeField] private List<Sprite> GameOverRobotPurgeData = new List<Sprite>();

    [Header("�����p�[�c���")]
    [SerializeField] private SpriteRenderer _partsPrefab;
    private SpriteRenderer _partsObject;
    [SerializeField] private Vector2 _propellerLocate = Vector2.zero;   // �v���y���̏o���ʒu
    [SerializeField] private Vector2 _rocketLocate = Vector2.zero;      // ���P�b�g�̏o���ʒu
    [SerializeField] private Vector2 _gliderLocate = Vector2.zero;      // �O���C�_�[�̏o���ʒu

    [SerializeField] private float _bombExplodeDistance = 0f;   // �����̏o������
    [SerializeField] private ParticleSystem _explodesPrefab;    // �����G�t�F�N�g��Prefab

    // �p�[�c�̎g�p�I���ƂƂ��ɔj������I�u�W�F�N�g�̃��X�g
    private List<GameObject> _destroyWithPartsObjects = new List<GameObject>();

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
        BuildUsePartsObject(performance, data);
        if (_usePartsEffect)
        {
            _usePartsEffect.gameObject.SetActive(true);
            _usePartsEffect.Play();
        }
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

        // SE��炷
        if (performance.usePartsSE != null && _usePartsAudioSource != null && performance.forceType != PartsPerformance.E_ForceType.Bomb)
        {
            _usePartsAudioSource.clip = performance.usePartsSE;
            _usePartsAudioSource.Play();
        }
        if (_purgePartsAudioSource != null && performance.purgePartsSE != null)
        {
            _purgePartsSE = performance.purgePartsSE;
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
        if (_partsObject) Destroy(_partsObject.gameObject);
        _animator.SetTrigger("EndUse");

        // SE�̏���
        if (_purgePartsAudioSource != null && _purgePartsSE != null)
        {
            _purgePartsAudioSource.PlayOneShot(_purgePartsSE);
            _purgePartsSE = null;
        }
        if (_usePartsAudioSource) _usePartsAudioSource.Stop();

        DestroyAllObjectsWithParts();
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
        if (_partsObject)
        {
            _purgeManager.AddPartsBySprite(usingPartsSprite);
            usingPartsSprite = null;
            Destroy(_partsObject.gameObject);
        }
        DestroyAllObjectsWithParts();
        if (_usePartsAudioSource) _usePartsAudioSource.Stop();
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

        // �Q�[�����s���̃A�j���[�V�����Ȃǂ̃��{�b�g�֌W�̏���
        _purgeManager.AddPartsBySprite(GameOverRobotPurgeData);
        if (_partsObject)
        {
            _purgeManager.AddPartsBySprite(usingPartsSprite);
            usingPartsSprite = null;
            Destroy(_partsObject.gameObject);
        }
        DestroyAllObjectsWithParts();
        if (_usePartsAudioSource) _usePartsAudioSource.Stop();
    }

    // �J�X�^�����j���[���J�����ۂɌĂяo����郁�\�b�h
    public void OpenCustomMenu()
    {
        if (PlaySceneController.Instance.IsOpenableCustomMenu)
        {
            bodyCollider.enabled = false;
            _status = E_RobotStatus.EndFly;

            if (_usePartsAudioSource) _usePartsAudioSource.Stop();

            if (_partsObject)
            {
                _purgeManager.AddPartsBySprite(usingPartsSprite);
                usingPartsSprite = null;
                Destroy(_partsObject.gameObject);
            }
            DestroyAllObjectsWithParts();
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

    // �p�[�c�f�[�^���瑕������p�[�c���\�z����
    public void BuildUsePartsObject(PartsPerformance performance, PartsInfo.PartsData data)
    {
        // �ʒu�Ɗp�x�𒲐�����
        Vector3 localPosition = Vector3.zero;
        float angle = data.angle - 90;
        switch (performance.forceType)
        {
            case PartsPerformance.E_ForceType.Rocket:
                localPosition += (Vector3)_rocketLocate;
                break;
            case PartsPerformance.E_ForceType.Propeller:
                localPosition += (Vector3)_propellerLocate;
                break;
            case PartsPerformance.E_ForceType.Glider:
                localPosition += (Vector3)_gliderLocate;
                angle += 90;
                break;
            // ���e�͑������Ă���u�Ԃ��قږ����̂ŁA�p�[�c�I�u�W�F�N�g�͐������Ȃ�
            case PartsPerformance.E_ForceType.Bomb:
                // ������������
                if (_explodesPrefab)
                {
                    Vector3 position = Quaternion.Euler(0, 0, data.angle - 180) * Vector3.right * _bombExplodeDistance;
                    var explodes = Instantiate(_explodesPrefab, transform.position + position, Quaternion.Euler(-90, 0, 0));
                    explodes.Play();
                    GimickManager.Instance.RegisterAsDeleteObject(explodes.gameObject);

                    // �g�p���̌��ʉ���炷
                    if(explodes.TryGetComponent(out AudioSource audiouSource))
                    {
                        audiouSource.clip = performance.usePartsSE;
                        audiouSource.Play();
                    }
                }
                return;
            default:
                return;
        }

        // �I�u�W�F�N�g�𐶐�����
        _partsObject = Instantiate(_partsPrefab, transform);
        _partsObject.transform.localPosition = localPosition;
        _partsObject.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        if (performance.animatorController)
        {
            // �A�j���[�^�[��ݒ肷��
            var partsAnimator = _partsObject.gameObject.AddComponent<Animator>();
            partsAnimator.runtimeAnimatorController = performance.animatorController;
        }
        else
        {
            // �X�v���C�g�i�Î~��j��ݒ肷��
            _partsObject.sprite = performance.partsSprite;
        }
    }

    // �p�[�c�̎g�p�I���Ƌ��ɔj�������p�[�c�̓o�^
    public void RegisterObjectAsDestroyWithParts(GameObject target)
    {
        _destroyWithPartsObjects.Add(target);
    }
    private void DestroyAllObjectsWithParts()
    {
        // �I���Ƌ��ɍ폜����I�u�W�F�N�g�̍폜
        for (int i = 0; i < _destroyWithPartsObjects.Count; i++)
        {
            if (_destroyWithPartsObjects[i])
            {
                Destroy(_destroyWithPartsObjects[i]);
            }
        }
        _destroyWithPartsObjects.Clear();
    }
}
