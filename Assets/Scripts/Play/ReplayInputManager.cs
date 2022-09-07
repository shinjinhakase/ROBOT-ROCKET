using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���݂̃��v���C�p�̃f�[�^���i�[����
public class ReplayInputManager : SingletonMonoBehaviourInSceneBase<ReplayInputManager>
{
    // ���݂̃v���C�̃��v���C�f�[�^
    [SerializeField] private ReplayData _data = new ReplayData();
    [SerializeField] private int TransformUpdateFrame = 50; // �ʒu�����X�V����Ԋu�iFixedUpdate�P�ʂł̃t���[���j
    private int frameCnt = -1;

    private PlaySceneController _sceneController;
    [SerializeField] private MainRobot robot;


    private void FixedUpdate()
    {
        if (frameCnt != -1) frameCnt++;
    }

    // �f�[�^�̏������i�V�[���Ǘ��ŃQ�[�����J�n�����Ɠ����ɌĂяo����A�g�p�p�[�c��񂪏����������j
    public void Ready()
    {
        _sceneController = PlaySceneController.Instance;
        _data.ReadyPartsInfo(_sceneController.StageNum);
    }
    // ���v���C�̋L�^���J�n����i���{�b�g�̓����n�߂Ƀt���[���J�E���g�����킹����ł���j
    public void StartMemory()
    {
        frameCnt = 0;
        StartCoroutine(GetRobotTransformInterval());
    }
    // �p�[�c�̎g�p���L�^�i�p�[�c�Ǘ����p�[�c���g�p�����ۂɌĂяo���j
    public void UseParts()
    {
        _data.RegisterUseParts(frameCnt);
    }
    // �p�[�c�̎g�p�I�����L�^�i���{�b�g�̏�ԊǗ����g�p�I����ԂɑJ�ڂ����ۂɌĂяo���j
    public void EndUseParts()
    {
        _data.RegisterEndUseParts(frameCnt);
    }
    // �p�[�c�̊l�����L�^�i�p�[�c�Ǘ����p�[�c���l�������ۂɌĂяo���j
    public void GetParts(PartsInfo.PartsData data)
    {
        _data.RegisterGetParts(frameCnt, data.id, data.angle);
    }
    // �͂��L�^�i�̓N���X���͂��~�߂����̎��g�A���邢�̓Q�[�����I���������̎c�����͂��L�^�j
    public void SetForce(IForce force)
    {
        _data.RegisterRobotForce(frameCnt, force);
    }
    // �Q�[�����ʂ��L�^�i�Q�[�����I�������ۂɋL�^�j
    public void SetResult()
    {
        _data.RegisterResult(frameCnt, _sceneController.Score);
        frameCnt = -1;
    }
    // ���v���C�f�[�^��ۑ�����
    [ContextMenu("Debug/Save")]
    public void Save()
    {
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
            if(_sceneController.scene != PlaySceneController.E_PlayScene.GamePlay)
            {
                yield break;
            }
            robot.GetTransform(out Vector2 position, out Vector2 velocity);
            _data.RegisterRobotTransform(frameCnt, position, velocity);
            yield return waitForSeconds;
        }
    }
}
