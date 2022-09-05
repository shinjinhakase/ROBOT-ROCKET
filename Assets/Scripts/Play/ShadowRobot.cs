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
    private ReplayData replayData;

    // ���v���C�p�ϐ�
    private bool IsStart = false;   // �J�n�t���O
    private int frameCnt = 0;   // �t���[���J�E���^
    private int partsNo = 0;    // �g�p���Ă���p�[�c�̐�
    private bool IsUsingParts => partsNo < readyPartsLength + getPartsLength && frameCnt == replayData.usePartsFrame[partsNo];


    private RobotStatus _status;
    private ForceMove _move;
    private PlayPartsManager _playPartsManager;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<RobotStatus>();
        _move = GetComponent<ForceMove>();
        _move.IsAcceptExternalForce = false;    // ���v���C�ȊO�̊O�͂͌덷�̌��ƂȂ邽�ߎ󂯓���Ȃ��ݒ�ɂ���
    }

    private void FixedUpdate()
    {
        if (IsStart)
        {
            // �p�[�c�g�p���[�V�����J�ڂ̏���
            if (IsUsingParts) UseParts();

            // TODO�F�ʒu�E���x�Ȃǂ̏C��

            // TODO�F�͊֌W�̏���

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
        ReplayDatas replayDatas = ReplayDatas.Instance;
        if (index >= replayDatas.Length)
        {
            throw new Exception("���v���C�f�[�^���ǂݍ��߂܂���B");
        } else if (replayNo != -1)
        {
            throw new Exception("���Ƀ��v���C�f�[�^��ǂݍ���ł��܂��B");
        }
        // ���v���C�f�[�^���擾����
        replayNo = index;
        replayData = replayDatas.GetData(index);
        readyPartsLength = replayData.readyPartsList.Count; // �������Ă����p�[�c�̐����擾
        getPartsLength = replayData.getPartsList.Count;     // �����Ŋl�������p�[�c�̐����擾
        frameCnt = 0;
    }

    // ���v���C�ł̍Č����J�n����
    public void StartGame()
    {
        _playPartsManager = PlayPartsManager.Instance;
        _status.startGame();
        IsStart = true;
    }

    // �p�[�c���g�p����
    private void UseParts()
    {
        // �g�p�����p�[�c�̐��\�E�J�X�^���f�[�^���擾����
        PartsInfo.PartsData data;
        if (partsNo < readyPartsLength)
        {
            // �p�ӂ��Ă����p�[�c���g�p����
            data = replayData.readyPartsList[partsNo];
        }
        else
        {
            // �����œ��肵���p�[�c���g�p����
            data = replayData.getPartsList[partsNo - readyPartsLength].buildPartsData();
        }
        PartsPerformance performance = _playPartsManager.GetPerformance(data.id);

        // �p�[�c�̎g�p��ԂɈڂ�i�A�j���[�V�����J�ځj
        //�i�ړ��Ɋւ��Ă͑��̗͂ƈꊇ�ŊǗ�����j
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
}
