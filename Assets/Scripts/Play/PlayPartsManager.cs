using System;
using UnityEngine;

// �v���C�V�[���Ŏg�p����A�C�e�����Ǘ�����Component�B
// �����ł̃A�C�e���̓���E�g�p�A�d�ʌv�Z�͂����������B
public class PlayPartsManager : SingletonMonoBehaviourInSceneBase<PlayPartsManager>
{
    private PartsInfo partsInfo;
    [SerializeField] private PartsPerformanceData partsPerformanceData;

    private void Start()
    {
        partsInfo = PartsInfo.Instance;
    }

    // ���d�ʂ��v�Z���ĕԂ�
    public float GetAllWeight()
    {
        float allWeight = 0f;
        var datas = partsInfo.GetPartsList();
        datas.ForEach(data => allWeight += partsPerformanceData.getData(data.id).m);
        return allWeight;
    }

    // �p�[�c���g���i�g���p�[�c�̃f�[�^�ƁA���܂��͂�Ԃ��j
    public void UseParts(out PartsInfo.PartsData data, out IForce force)
    {
        // �g�p����p�[�c�̃f�[�^���擾����
        if (partsInfo.Length == 0) throw new Exception("�g�p����p�[�c������܂���B");
        data = partsInfo.GetParts(0);
        var performance = partsPerformanceData.getData(data.id);
        // ���X�g����A�C�e�������O����
        partsInfo.RemoveParts();

        // ������͂��\�z����
        switch (performance.forceType)
        {
            case PartsPerformance.E_ForceType.Bomb:
                force = new ImpulseForce(data.angle, performance.F, performance.m);
                break;
            case PartsPerformance.E_ForceType.Rocket:
            case PartsPerformance.E_ForceType.Propeller:
                force = new PressForce(data.angle, performance.F, performance.t, performance.k, performance.m);
                break;
            case PartsPerformance.E_ForceType.Glider:
            case PartsPerformance.E_ForceType.CollisionForce:
            default:
                throw new Exception("���{�b�g�ɉ�����͂��\�z�ł��܂���B");
        }
    }

    // �f�o�b�O�p�B�g�p�p�[�c���X�g�Ƀe�X�g�p�[�c��ǉ�����B
    [ContextMenu("Debug/GetTestParts")]
    private void AddTestParts()
    {
        var data = new PartsInfo.PartsData();
        data.id = PartsPerformance.E_PartsID.TestParts;
        data.angle = 80;
        partsInfo.AddParts(data);
    }
}
