using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����̃V���h�E���Ǘ�����Component
public class ShadowManager : SingletonMonoBehaviourInSceneBase<ShadowManager>
{
    // �V���h�E���{�b�g��Prefab
    [SerializeField] private ShadowRobot shadowPrefab;

    // �V�[����ɂ���V���h�E���{�b�g�I�u�W�F�N�g�̃��X�g
    private bool IsStart = false;
    public List<int> replayIndexList = new List<int>();
    private List<ShadowRobot> shadows = new List<ShadowRobot>();


    // �w��̃��v���C�̃V���h�E��ǉ�����
    [ContextMenu("Debug/RegisterShadow")]
    public void RegisterShadow()
    {
        if (IsStart) return;
        foreach(var index in replayIndexList)
        {
            var shadow = Instantiate(shadowPrefab);
            shadow.LoadReplayData(index);
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
        if (shadow != null) Destroy(shadow);
    }

    // �S�ẴV���h�E���~������
    public void StopAllShadow()
    {
        for(int i = 0; i < shadows.Count; i++)
        {
            Destroy(shadows[i]);
        }
        shadows.Clear();
        IsStart = false;
    }
}
