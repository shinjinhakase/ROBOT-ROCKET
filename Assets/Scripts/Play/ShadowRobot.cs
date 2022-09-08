using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �V���h�E�̓�������������Component�i�����̋O�Ղ����Č�����j
[RequireComponent(typeof(ForceMove))]
[RequireComponent(typeof(ReplayPlayer))]
public class ShadowRobot : MonoBehaviour
{
    // �L���b�V��
    [SerializeField] private RobotStatus _status;
    private ForceMove _move;
    private ReplayPlayer _player;
    private PlayPartsManager _playPartsManager;

    void Awake()
    {
        _move = GetComponent<ForceMove>();
        _move.IsAcceptExternalForce = false;    // ���v���C�ȊO�̊O�͂͌덷�̌��ƂȂ邽�ߎ󂯓���Ȃ��ݒ�ɂ���
        _player = GetComponent<ReplayPlayer>();
    }

    // ���v���C�f�[�^��ǂݍ���
    public void LoadReplayData(int index)
    {
        _player.LoadReplayData(index);
    }

    // ���v���C�̍Č����J�n����
    public void StartGame()
    {
        // �������Ă����p�[�c�̏����d�ʂ����{�b�g�ɓK�p
        _playPartsManager = PlayPartsManager.Instance;
        SetInitialWeight();

        _status.startGame();
        _player.StartReplay();
    }

    // �p�[�c���g�p�^�C�~���O�ŌĂяo����郁�\�b�h
    public void UseParts(PartsInfo.PartsData data)
    {
        // �g�p�����p�[�c�̐��\���擾����
        PartsPerformance performance = _playPartsManager.GetPerformance(data.id);

        // �p�[�c�̎g�p��ԂɈڂ�i�A�j���[�V�����J�ځj
        if (_status.IsUsingParts)
        {
            Debug.LogWarning("ReplayPlayer�̊O���ŋ����I�Ƀp�[�c�̎g�p���~���܂����B");
            _status.endUseParts();
        }
        _status.endCooltime();
        _status.startUseParts(performance, data);
    }

    // �p�[�c�̎g�p�I���^�C�~���O�ŌĂяo����郁�\�b�h
    public void endUseParts()
    {
        _status.endUseParts();
    }

    // �擾�����p�[�c�̏d�ʂ����{�b�g�ɒǉ�����
    public void AddGettingPartsWeight(PartsInfo.PartsData data)
    {
        // �l�������p�[�c�̏d�ʂ��擾����
        float weight = _playPartsManager.GetPerformance(data.id).m;

        // ���{�b�g�̏d�ʂ��X�V����
        _move.SetWeight(_move.GetWeight() + weight);
    }

    // ���v���C�I�����̏���
    public void ReplayFinish()
    {
        // �͂𖳂����A�Q�[���I�[�o�[���[�V�������o������Ŏ��g���폜����
        _move.ZeroForce();
        _status.GameOver();
        ShadowManager.Instance.FinishReplay(this);
    }


    // �J�n���̏������ʂ�ݒ肷��
    private void SetInitialWeight()
    {
        _move.SetWeight(_player.GetInitialWeight(_playPartsManager));
    }
}
