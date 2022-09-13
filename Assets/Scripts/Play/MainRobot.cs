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

    private float _highScore = 0;   // �ō����B����

    private bool IsNotStart = false;        // ��s���n�߂��^�C�~���O���v�邽�߂́A��s���Ă��Ȃ����t���O
    private bool IsUsePartsInForce = false; // �A�C�e���������I�Ɏg�p���邩�̃t���O�i���v���C�ȂǂŐ�����������Ȃ��悤�Ɂj
    private bool ReplayMode = false;        // ���v���C����Ɉڂ邩�̃t���O
    private ReplayData _useReplayData;

    private void Awake()
    {
        partsInfo = PartsInfo.Instance;
        _move = GetComponent<ForceMove>();
        _player = GetComponent<ReplayPlayer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // ��s���̏���
        if (_status.IsFlying && !_player.IsPlaying)
        {
            // �A�C�e���g�p�I������
            if (_status.IsUsingParts && !playPartsManager.IsUsingParts)
            {
                endUseParts();
            }

            // ���̑��쏈���i�A�C�e���g�p�j
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("�A�C�e���g�p�{�^����������");
                UsePartsByControl();
            }
            // �A�C�e���̎蓮�p�[�W
            if (playPartsManager.IsUsingParts && Input.GetKeyDown(KeyCode.R))
            {
                playPartsManager.IsUsingParts = false;
            }
        }
        // ��s���n�ߔ���
        else if (IsNotStart &&  playSceneController.scene == PlaySceneController.E_PlayScene.GamePlay && _status.IsWaitingForFly && Input.GetKeyDown(KeyCode.Space))
        {
            IsNotStart = false;
            RobotStartMove();
            UsePartsByControl();
        }

        // �q�b�g�X�g�b�v�f�o�b�O
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlaySceneController.Instance.RequestHitStopBySlow(0.25f, 1f);
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            PlaySceneController.Instance.RequestHitStopByStop(1f);
        }
    }

    private void FixedUpdate()
    {
        // �ō����B�������m�F����
        if (_status.IsFlying && !_player.IsPlaying && transform.position.x > _highScore)
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

        if (ReplayMode)
        {
            // ���v���C�̏����ݒ�
            if (_useReplayData == null) throw new Exception("���v���C�p�̃f�[�^���ݒ肳��Ă��܂���B");
            _player.LoadReplayData(_useReplayData);

            // �p�ӂ��Ă����p�[�c�����v���C�̂��̂ɕύX
            partsInfo.partsList = _player.InitialPartsDatas;

            // �X�R�A�����v���C�̂��̂ɕύX
            playSceneController.Score = _useReplayData.score;

            // ���v���C�p�̏������[�h�ɕύX
            IsUsePartsInForce = true;
            IsNotStart = false;
            Invoke("RobotStartMove", 1f);
        }
        else
        {
            // ���삷��ۂ̏���
            IsUsePartsInForce = false;
            IsNotStart = true;  // �܂���s���J�n���Ă��Ȃ��t���O��true�ɂ���
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

        // ���{�b�g�������n�߂��ۂ̃C�x���g���Ă�
        playSceneController.RobotStartMove();
        if (_player.IsLoaded) _player.StartReplay();
    }

    // ����ɂ���ăA�C�e�����g�p���鏈��
    [ContextMenu("Debug/UseParts")]
    private void UsePartsByControl()
    {
        if (_player.IsPlaying) return;
        else if (!partsInfo.HasNext) return;
        else if (!_status.IsPartsUsable) return;
        playPartsManager.UseParts(out PartsPerformance performance, out PartsInfo.PartsData data, out IForce force);
        UseParts(data, performance, force);
    }
    // ���v���C�ɂ���ăA�C�e�����g�p���鏈��
    public void UsePartsByReplay(PartsInfo.PartsData data)
    {
        if (!_player.IsPlaying) return;
        UseParts(data, playPartsManager.GetPerformance(data.id));
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
        else if (!_status.IsPartsUsable && IsUsePartsInForce)
        {
            // �A�C�e�����g�p�ł����Ԃɋ����I�Ɉڍs����
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
        }
    }
    // �p�[�c�̎g�p���I������ۂ̏���
    public void endUseParts()
    {
        _status.endUseParts();
    }


    // �Q�[���N���A���̏���
    public void GameClear()
    {
        ReplayMode = false;
        // �͂𖳂����A�����A�j���[�V���������ɑJ�ڂ���
        _move.ZeroForce();
        _status.GameClear();
    }

    // �Q�[���I�[�o�[���̏���
    public void GameOver()
    {
        ReplayMode = false;
        // ���s�A�j���[�V���������ɑJ�ڂ���
        _move.ZeroForce();
        _status.GameOver();

        // ���{�b�g���\���ɂ���i�p�[�W�̃p�[�c����юU��A�j���[�V�����Ɉڂ�j
        gameObject.SetActive(false);
    }

    // �J�X�^�����j���[���J�����Ƃ��̏���
    public void OpenCustomMenu()
    {
        // ��s���ɌĂяo���ꂽ�Ȃ�A�N���[���Ŏ����グ����A�j���[�V����������
        if (_status.IsFlying)
        {
            ReplayMode = false;
            _player.StopReplayInForce();
            _move.OpenCustomMove();
            _status.OpenCustomMenu();
        }
        // �i�Q�[���I�[�o�[��ɌĂяo���ꂽ�Ȃ�A���ɔ�\���Ȃ̂ŉ������Ȃ��j
    }

    // ���Z�b�g����Ƃ��̏���
    public void ResetToStart()
    {
        _highScore = 0;
        partsInfo = PartsInfo.Instance;
        _move.ResetToFirst();
        _status.ResetStatus();
        gameObject.SetActive(true);
    }

    // ���v���C���Đ�����ۂ̏���
    public void SetReplayMode()
    {
        ReplayMode = true;
        if (_useReplayData == null)
        {
            // ���v���C�f�[�^����Ȃ�A���݂̃��v���C�f�[�^���擾����
            _useReplayData = new ReplayData(ReplayInputManager.Instance.Data);
        }
    }


    // �Q�[���I�[�o�[�ƂȂ铖���蔻��Ƃ̏Փ˔����S�����\�b�h
    public void CheckGameOverCollision(Collider2D other)
    {
        if (!_player.IsPlaying && _status.IsFlying && other.CompareTag(GameOverColliderTag))
        {
            PlaySceneController.Instance.GameOver();
        }
    }
    // ���v���C�Ɏ����̈ʒu�Ƒ��x�����i�[����
    public void GetTransform(out Vector2 position, out Vector2 velocity)
    {
        position = transform.position;
        velocity = _move.GetVelocity();
    }
}
