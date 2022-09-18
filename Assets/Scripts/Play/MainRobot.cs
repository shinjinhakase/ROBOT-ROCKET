using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �A�C�e�����g�p���ė͂������ē����A�Q�[���̃��C�����{�b�g
[RequireComponent(typeof(ForceMove))]
[RequireComponent(typeof(ReplayPlayer))]
public class MainRobot : MonoBehaviour
{
    private static string GameOverColliderTag = "GameOverCollider"; // �Q�[���I�[�o�[�ƂȂ铖���蔻��ɕt����^�O�̖��O

    private ReplayPlayer _player;
    private PartsInfo partsInfo;
    private PlaySceneController playSceneController;
    private PlayPartsManager playPartsManager;
    public RobotStatus _status;
    [HideInInspector] public ForceMove _move;

    private ReplayData _useReplayData;  // ���v���C�Đ����ɗp����f�[�^
    private float _highScore = 0;       // �ō����B����
    [SerializeField] private float _underline;  // ��������Q�[���I�[�o�[�ƂȂ����x

    [SerializeField] private ParticleSystem _gameoverPerticle;

    // �V�[������
    private bool IsWaitForControl => playSceneController ? playSceneController.IsWaitingForRobot && !playSceneController.IsReplayMode : false;
    private bool IsControlMode => playSceneController ? playSceneController.IsRobotStartMove && playSceneController.IsPlayingGame && !playSceneController.IsReplayMode : false;
    private bool IsReplayMode => playSceneController ? playSceneController.IsReplayMode : throw new NullReferenceException("PlaySceneController��Awake�����O��Instance���Q�Ƃ��܂���");

    private void Awake()
    {
        partsInfo = PartsInfo.Instance;
        _move = GetComponent<ForceMove>();
        _player = GetComponent<ReplayPlayer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // ��s���̑��샂�[�h������
        if (IsControlMode)
        {
            // �A�C�e���g�p�I������
            if (_status.IsUsingParts && !playPartsManager.IsUsingParts)
            {
                endUseParts();
            }

            // �A�C�e���g�p�L�[�̓��͎�t
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UsePartsByControl();
            }
            // �A�C�e���̎蓮�p�[�W����
            if (playPartsManager.IsUsingParts && Input.GetKeyDown(KeyCode.R))
            {
                playPartsManager.IsUsingParts = false;
            }

            // ���x�ɂ��Q�[���I�[�o�[����
            if (transform.position.y <= _underline)
            {
                playSceneController.GameOver();
            }
        }
        // ��s���n�ߔ���
        else if (IsWaitForControl && partsInfo.HasNext && Input.GetKeyDown(KeyCode.Space))
        {
            RobotStartMove();
            UsePartsByControl();
        }
    }

    private void FixedUpdate()
    {
        // �ō����B�������m�F����
        if (IsControlMode && transform.position.x > _highScore)
        {
            _highScore = transform.position.x;
            playSceneController.Score = _highScore;
            // �N���A����
            if (_highScore >= playSceneController.GoalXPoint)
            {
                playSceneController.GameClear();
            }
        }
    }

    // �Q�[���J�n���\�b�h
    public void GameStart()
    {
        // �L���b�V�����擾
        playSceneController = PlaySceneController.Instance;
        playPartsManager = PlayPartsManager.Instance;

        if (IsReplayMode)
        {
            // ���v���C�̏����ݒ�
            if (_useReplayData == null) throw new Exception("���v���C�p�̃f�[�^���ݒ肳��Ă��܂���B");
            _player.LoadReplayData(_useReplayData);

            // �p�ӂ��Ă����p�[�c�����v���C�̂��̂ɕύX
            partsInfo.partsList = _player.InitialPartsDatas;

            // �X�R�A�����v���C�̂��̂ɕύX
            playSceneController.Score = _useReplayData.score;

            // ���v���C����1�b��ɓ����n�߂�
            Invoke("RobotStartMove", 1f);
        }
        else
        {
            // ���v���C���Ɏg�p����f�[�^������������
            _useReplayData = null;
        }
    }
    // ���{�b�g�������n�߂��ۂɌĂ΂�郁�\�b�h
    private void RobotStartMove()
    {
        // ���{�b�g�̏����d�ʂ�ݒ肷��
        float allWeight = playPartsManager.GetAllWeight();
        _move.SetWeight(allWeight + ForceMove.RobotWeight);

        // ��Ԃ�ω�������
        _status.startGame();

        // ���{�b�g�������n�߂��ۂ̏�����ԂɈڂ�
        playSceneController.RobotStartMove();
        if (_player.IsLoaded) _player.StartReplay();
    }

    // ����ɂ���ăA�C�e�����g�p���鏈��
    [ContextMenu("Debug/UseParts")]
    private void UsePartsByControl()
    {
        if (playSceneController.IsReplayMode) return;
        else if (!partsInfo.HasNext) return;
        else if (!_status.IsPartsUsable) return;
        playPartsManager.UseParts(out PartsPerformance performance, out PartsInfo.PartsData data, out IForce force);
        UseParts(data, performance, force);
    }
    // ���v���C�ɂ���ăA�C�e�����g�p���鏈��
    public void UsePartsByReplay(PartsInfo.PartsData data)
    {
        if (!playSceneController.IsReplayMode) return;
        playPartsManager.UseParts(out PartsPerformance performance, out _, out _);
        UseParts(data, performance);
    }
    // �p�[�c�g�p���̋��ʏ���
    private void UseParts(PartsInfo.PartsData data, PartsPerformance performance, IForce force = null)
    {
        // �A�C�e�����g���邩����
        if (!_status.IsFlying)
        {
            Debug.LogWarning("��s���ȊO�ɃA�C�e�����g�p���悤�Ƃ��Ă��܂��I");
            return;
        }
        // �A�C�e�����g�p�ł��Ȃ���Ԃ̂Ƃ�
        else if (!_status.IsPartsUsable && IsReplayMode)
        {
            // ���v���C���̓A�C�e�����g�p�ł����Ԃɋ����I�Ɉڍs����
            if (_status.IsUsingParts) _status.endUseParts();
            _status.endCooltime();
        }

        // ��ԊǗ��ɃA�C�e���̎g�p��`����
        _status.startUseParts(performance, data);

        // �����Ǘ��ɗ͂�������
        _move.AddForce(force);

        // �����I�u�W�F�N�g����������
        Transform _transform = transform;
        Vector3 nowPosition = _transform.position;
        foreach (SummonableObject summonObject in performance.summonObjects)
        {
            var summonned = Instantiate(summonObject, nowPosition, Quaternion.identity);
            summonned.Summon(data, _transform);
            if (summonned.IsDestroyWithParts)
            {
                // �ݒ肪�I���ł���΁A�p�[�c�ƂƂ��ɔj�������悤�ɓo�^����
                _status.RegisterObjectAsDestroyWithParts(summonned.gameObject);
            }
        }

        Debug.Log("(MainRobot)�p�[�c���g�p���܂����FID = " + data.id);
    }
    // �p�[�c�̎g�p���I������ۂ̏���
    public void endUseParts()
    {
        PartsInfo.Instance.RemoveParts();   // �p�[�c�����X�g����폜����
        _status.endUseParts();
        Debug.Log("(MainRobot)�p�[�c�̎g�p���I�����܂���");
    }


    // �Q�[���N���A���̏���
    public void GameClear()
    {
        // �͂𖳂����A�����A�j���[�V���������ɑJ�ڂ���
        _move.ZeroForce();
        _status.GameClear();
    }

    // �Q�[���I�[�o�[���̏���
    public void GameOver()
    {
        // ���s�A�j���[�V���������ɑJ�ڂ���
        _move.ZeroForce();
        _status.GameOver();

        // �G�t�F�N�g��\������
        var effect = Instantiate(_gameoverPerticle, transform.position, Quaternion.Euler(-90, 0, 0));
        GimickManager.Instance.RegisterAsDeleteObject(effect.gameObject);

        // ���{�b�g���\���ɂ���i�p�[�W�̃p�[�c����юU��A�j���[�V�����Ɉڂ�j
        gameObject.SetActive(false);
    }

    // �J�X�^�����j���[���J�����Ƃ��̏���
    public void OpenCustomMenu()
    {
        // ��s���ɌĂяo���ꂽ�Ȃ�A�Q�[���I�[�o�[���̔��j�������Ă�
        if (playSceneController ? playSceneController.IsRobotStartMove && _status.IsFlying : false)
        {
            GameOver();
        }
        // �i�Q�[���I����ɌĂяo���ꂽ�Ȃ牽�����Ȃ��j
    }

    // �X�^�[�g�n�_�ɖ߂�A��Ԃ����Z�b�g����
    public void ResetToStart()
    {
        _highScore = 0;
        partsInfo = PartsInfo.Instance;
        _move.ResetToFirst();
        gameObject.SetActive(true);
        _status.ResetStatus();
    }

    // ���v���C���Đ�����ۂ̏���
    public void SetReplayData()
    {
        if (_useReplayData == null)
        {
            // ���v���C�f�[�^����Ȃ�A���݂̃��v���C�f�[�^���擾����
            _useReplayData = ReplayInputManager.Instance.Data;
        }
    }


    // �Q�[���I�[�o�[�ƂȂ铖���蔻��Ƃ̏Փ˔����S�����\�b�h
    public void CheckGameOverCollision(Collider2D other)
    {
        if (IsControlMode && other.CompareTag(GameOverColliderTag))
        {
            if (other.TryGetComponent(out GameOverSE gameoverSE))
            {
                // �Q�[���I�[�o�[�p��SE���ݒ肳��Ă���Ȃ�炷�B
                gameoverSE.Beep();
            }
            PlaySceneController.Instance.GameOver();
        }
    }
    // ���v���C�p�Ɏ����̈ʒu�Ƒ��x����ԓ�����
    public void GetTransform(out Vector2 position, out Vector2 velocity)
    {
        position = transform.position;
        velocity = _move.GetVelocity();
    }
}
