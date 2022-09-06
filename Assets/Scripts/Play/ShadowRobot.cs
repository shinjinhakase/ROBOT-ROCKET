using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �V���h�E�̓�������������Component�i�����̋O�Ղ����Č�����j
[RequireComponent(typeof(RobotStatus))]
[RequireComponent(typeof(ForceMove))]
public class ShadowRobot : MonoBehaviour
{
    // ���v���C�f�[�^
    private int replayNo = -1;
    private int readyPartsLength = 0;
    private int getPartsLength = 0;
    private int transformLength = 0;
    private ReplayData replayData;

    // ���v���C�p�ϐ�
    private bool IsStart = false;   // �J�n�t���O
    private int frameCnt = 0;   // �t���[���J�E���^

    // �g�p�p�[�c�֘A
    private PartsInfo.PartsData GetPartsData(int index) => index < readyPartsLength ? replayData.readyPartsList[index] : replayData.getPartsList[index - readyPartsLength].buildPartsData();
    private int partsNo = 0;    // �g�p���Ă���p�[�c�̐�
    private bool IsUsingParts => partsNo < readyPartsLength + getPartsLength && frameCnt >= replayData.usePartsFrame[partsNo];  // �p�[�c���g�p����t���[��������

    // �ʒu���֘A
    private int transCnt = 0;   // �ʒu���̎Q�ƃC���f�b�N�X
    private ReplayData.LocateData nextLocateData = null;
    private ReplayData.LocateData GetNextLocate() => transCnt < transformLength ? replayData.locateDatas[transCnt] : null;  // ���̍X�V�f�[�^���擾
    private bool IsTransformUpdating => nextLocateData != null && frameCnt >= nextLocateData.frame; // ���W���X�V�����t���[��������

    // �͊֘A
    private List<ReplayData.ForceData> GetAddForces() => replayData.forceDatas.FindAll(data => frameCnt == data.frame); // ���̃t���[���ŉ�������͂̃f�[�^���擾

    // �L���b�V��
    private RobotStatus _status;
    private ForceMove _move;
    private PlayPartsManager _playPartsManager;
    private Rigidbody2D _rb;

    void Awake()
    {
        _status = GetComponent<RobotStatus>();
        _move = GetComponent<ForceMove>();
        _move.IsAcceptExternalForce = false;    // ���v���C�ȊO�̊O�͂͌덷�̌��ƂȂ邽�ߎ󂯓���Ȃ��ݒ�ɂ���
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsStart)
        {
            // �p�[�c�g�p���[�V�����J�ڂ̏���
            if (IsUsingParts) UseParts();

            // �ʒu�E���x�Ȃǂ̒���I�ȏC��
            if (IsTransformUpdating)
            {
                // �f�[�^�Ɋ�Â��Ĉʒu�Ƒ��x���C������
                transform.position = nextLocateData.position;
                _rb.velocity = nextLocateData.velocity;
                transCnt++;
                nextLocateData = GetNextLocate();
            }

            // �p�[�c�l�����̎��ʕω�����
            AddGettingPartsWeight();
            // �͊֌W�̏���
            var forceDatas = GetAddForces();
            if (forceDatas?.Count > 0)
            {
                // ������ꂽ�͂��f�[�^���畜������ForceMove�ɗ^����
                foreach(var forceData in forceDatas)
                {
                    var force = forceData.buildForce();
                    _move.AddForce(force);
                }
            }

            // ���v���C�̏I������
            if(frameCnt >= replayData.finishFrame)
            {
                ReplayFinish();
            }

            frameCnt++;
        }
    }

    // ���v���C�f�[�^��ǂݍ���
    public void LoadReplayData(int index)
    {
        // ���v���C�f�[�^���擾����
        ReplayDatas replayDatas = ReplayDatas.Instance;
        if (index >= replayDatas.Length)
        {
            throw new Exception("���v���C�f�[�^���ǂݍ��߂܂���B");
        } else if (replayNo != -1)
        {
            throw new Exception("���Ƀ��v���C�f�[�^��ǂݍ���ł��܂��B");
        }
        replayNo = index;
        replayData = replayDatas.GetData(index);

        // �p�[�c���̎擾
        readyPartsLength = replayData.readyPartsList.Count; // �������Ă����p�[�c�̐����擾
        getPartsLength = replayData.getPartsList.Count;     // �����Ŋl�������p�[�c�̐����擾
        partsNo = 0;
        // �ʒu�E�͏��̎擾
        transformLength = replayData.locateDatas.Count;     // �X�V�ʒu���̌������擾
        transCnt = 0;
        nextLocateData = GetNextLocate();                   // ���ɍX�V������W�E���x�̃f�[�^���擾
        frameCnt = 0;
    }

    // ���v���C�̍Č����J�n����
    public void StartGame()
    {
        // �������Ă����p�[�c�̏����d�ʂ����{�b�g�ɓK�p
        _playPartsManager = PlayPartsManager.Instance;
        SetInitialWeight();

        _status.startGame();
        IsStart = true;
    }

    // �p�[�c���g�p���Ă����t���[���ŌĂяo����郁�\�b�h
    private void UseParts()
    {
        // �g�p�����p�[�c�̐��\�E�J�X�^���f�[�^���擾����
        PartsInfo.PartsData data = GetPartsData(partsNo);
        PartsPerformance performance = _playPartsManager.GetPerformance(data.id);

        // �p�[�c�̎g�p��ԂɈڂ�i�A�j���[�V�����J�ځj
        _status.startUseParts(performance, data);

        partsNo++;
    }

    // ���v���C�I�����̏���
    public void ReplayFinish()
    {
        // �͂𖳂����A�Q�[���I�[�o�[���[�V�������o������Ŏ��g���폜����
        _move.ZeroForce();
        _status.GameOver();
        IsStart = false;    // �ǂ����j������邯�ǈꉞ
        ShadowManager.Instance.FinishReplay(this);
    }



    // �J�n���̏������ʂ�ݒ肷��
    private void SetInitialWeight()
    {
        float sumWeight = 0;
        foreach (var readyPartsData in replayData.readyPartsList)
        {
            sumWeight += _playPartsManager.GetPerformance(readyPartsData.id).m;
        }
        _move.SetWeight(sumWeight + ForceMove.RobotWeight);
    }

    // ���݂̃t���[���Ŏ擾�����p�[�c�̏d�ʂ����{�b�g�ɒǉ�����
    private void AddGettingPartsWeight()
    {
        // ���݂̃t���[���Ŋl�������p�[�c�̃f�[�^���擾
        List<ReplayData.GetPartsData> partsDatas = replayData.getPartsList.FindAll(data => frameCnt == data.frame);
        if (partsDatas?.Count == 0) return;

        // �擾�����p�[�c�̑��d�ʂ��v�Z����
        float sumWeight = 0;
        foreach(var data in partsDatas)
        {
            sumWeight += _playPartsManager.GetPerformance(data.id).m;
        }

        // ���{�b�g�̏d�ʂ��X�V����
        _move.SetWeight(_move.GetWeight() + sumWeight);
    }
}
