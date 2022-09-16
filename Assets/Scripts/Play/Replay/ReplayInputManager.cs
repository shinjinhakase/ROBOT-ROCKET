using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// ���݂̃��v���C�p�̃f�[�^���i�[����
public class ReplayInputManager : SingletonMonoBehaviourInSceneBase<ReplayInputManager>
{
    // ���݂̃v���C�̃��v���C�f�[�^
    [SerializeField] private ReplayData _data = new ReplayData();
    public ReplayData Data => _data;
    [SerializeField] private int TransformUpdateFrame = 50; // �ʒu�����X�V����Ԋu�iFixedUpdate�P�ʂł̃t���[���j
    private int frameCnt = -1;

    private PlaySceneController _sceneController;
    [SerializeField] private MainRobot robot;

    private bool NoMemoryMode = false;
    private bool IsGamePlay = false;

    private void FixedUpdate()
    {
        if (IsGamePlay) frameCnt++;
    }

    // ���v���C�f�[�^��ۑ������A���݂���f�[�^�����̂܂܂ɂ����Ԃɂ���
    public void SetNoMemoryMode()
    {
        if (IsGamePlay) return;
        NoMemoryMode = true;
    }
    // �f�[�^�̏������i�V�[���Ǘ��ŃQ�[�����J�n�����Ɠ����ɌĂяo����A�g�p�p�[�c��񂪏����������j
    public void Ready()
    {
        _sceneController = PlaySceneController.Instance;
        if (NoMemoryMode) return;
        _data.ReadyPartsInfo(_sceneController.StageNum);
    }
    // ���v���C�̋L�^���J�n����i���{�b�g�̓����n�߂Ƀt���[���J�E���g�����킹����ł���j
    public void StartMemory()
    {
        frameCnt = 0;
        IsGamePlay = true;
        if (NoMemoryMode) return;
        StartCoroutine(GetRobotTransformInterval());
    }
    // �p�[�c�̎g�p���L�^�i�p�[�c�Ǘ����p�[�c���g�p�����ۂɌĂяo���j
    public void UseParts()
    {
        if (NoMemoryMode) return;
        _data.RegisterUseParts(frameCnt);
    }
    // �p�[�c�̎g�p�I�����L�^�i���{�b�g�̏�ԊǗ����g�p�I����ԂɑJ�ڂ����ۂɌĂяo���j
    public void EndUseParts()
    {
        if (NoMemoryMode) return;
        _data.RegisterEndUseParts(frameCnt);
    }
    // �p�[�c�̊l�����L�^�i�p�[�c�Ǘ����p�[�c���l�������ۂɌĂяo���j
    public void GetParts(PartsInfo.PartsData data)
    {
        if (NoMemoryMode) return;
        _data.RegisterGetParts(frameCnt, data.id, data.angle);
    }
    // �͂��L�^�i�̓N���X���͂��~�߂����̎��g�A���邢�̓Q�[�����I���������̎c�����͂��L�^�j
    public void SetForce(IForce force)
    {
        if (NoMemoryMode) return;
        _data.RegisterRobotForce(frameCnt, force);
    }
    // �Q�[�����ʂ��L�^�i�Q�[�����I�������ۂɋL�^�j
    public void SetResult()
    {
        if (!NoMemoryMode && !PlaySceneController.Instance.IsWaitingForRobot)
        {
            // ���v���C���[�h�ł͂Ȃ��A���{�b�g�������n�߂Ă������́A�擾�����f�[�^�������ŕۑ������Ă݂�
            _data.RegisterResult(frameCnt, _sceneController.Score);
            Save();
        }
        IsGamePlay = false;
        NoMemoryMode = false;
    }
    // ���v���C�f�[�^��ۑ�����
    [ContextMenu("Debug/Save")]
    public void Save()
    {
        // ���{�b�g�������n�߂Ă��Ȃ��Ȃ�f�[�^��ۑ����Ȃ�
        ReplayDatas datas = ReplayDatas.Instance;
        datas.RegisterData(_data);
        datas.Save();
    }

    // ����I�Ƀ��{�b�g�ʒu���L�^����
    private IEnumerator GetRobotTransformInterval()
    {
        var waitForSeconds = new WaitForSeconds(TransformUpdateFrame * Time.fixedDeltaTime);
        while (true)
        {
            if (!_sceneController.IsPlayingGame)
            {
                yield break;
            }
            robot.GetTransform(out Vector2 position, out Vector2 velocity);
            _data.RegisterRobotTransform(frameCnt, position, velocity);
            yield return waitForSeconds;
        }
    }

    public string GetTweetText()
    {
        float finishTime = _data.finishFrame * Time.fixedDeltaTime;
        return "�X�e�[�W" + _data.StageNum + "�ɂāA" + _data.readyPartsList.Count + "�̃p�[�c��p����" + finishTime + "�b��" + _data.score + "m�܂Ŕ�ׂ܂����I\n";
    }
}
