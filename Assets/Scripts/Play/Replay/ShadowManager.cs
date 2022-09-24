using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����̃V���h�E���Ǘ�����Component
public class ShadowManager : SingletonMonoBehaviourInSceneBase<ShadowManager>
{
    // �V���h�E���{�b�g��Prefab
    [SerializeField] private ShadowRobot shadowPrefab;
    [SerializeField] private Vector2 firstPosition = new Vector2(0, -2.5f);

    // �V�[����ɂ���V���h�E���{�b�g�I�u�W�F�N�g�̃��X�g
    private bool IsStart = false;
    private List<ReplayData> _useReplayDatas = new List<ReplayData>();
    private List<ShadowRobot> shadows = new List<ShadowRobot>();


    // �w��̃��v���C�̃V���h�E��ǉ�����
    [ContextMenu("Debug/RegisterShadow")]
    public void RegisterShadow()
    {
        if (IsStart) return;

        // ���v���C���[�h�ł͖����Ƃ��A�V���h�E�ɗp���郊�v���C�f�[�^���X�V����
        if (!PlaySceneController.Instance.IsReplayMode)
        {
            // ���̃X�e�[�W�̃��v���C�f�[�^��S�ăV���h�E�ɂ���
            _useReplayDatas = new List<ReplayData>(StageReplayDatas);
        }

        // ���̃X�e�[�W�̃��v���C�f�[�^��S�ăV���h�E�ɂ���
        foreach (var replayData in _useReplayDatas)
        {
            var shadow = Instantiate(shadowPrefab, firstPosition, Quaternion.identity);
            shadow.LoadReplayData(replayData);
            shadows.Add(shadow);
        }
    }

    // �V���h�E�̃��v���C���J�n����
    public void StartShadowReplay()
    {
        if (IsStart) return;
        IsStart = true;
        foreach(var shadow in shadows)
        {
            shadow.StartGame();
        }
    }

    // �V���h�E�̃��v���C���I�������ۂɌĂяo����鏈��
    public void FinishReplay(ShadowRobot shadow)
    {
        // �V���h�E�����X�g����폜���A�I�u�W�F�N�g��j������
        shadows.Remove(shadow);
        if (shadow != null) Destroy(shadow.gameObject);
    }

    // �S�ẴV���h�E���~������
    public void StopAllShadow()
    {
        for(int i = 0; i < shadows.Count; i++)
        {
            Destroy(shadows[i].gameObject);
        }
        shadows.Clear();
        IsStart = false;
    }


    public void ResetOnGround()
    {
        foreach(var shadow in shadows)
        {
            shadow?.ResetOnGround();
        }
    }

    // �V���h�E�Ɏg�p���郊�v���C�f�[�^��I������
    private List<ReplayData> StageReplayDatas => ReplayDatas.Instance.GetStageReplay(PlaySceneController.Instance.StageNum);
}
