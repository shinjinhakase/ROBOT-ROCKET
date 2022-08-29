using System;
using UnityEngine;

public class PlayPartsManager : SingletonMonoBehaviourInSceneBase<PlayPartsManager>
{
    [SerializeField] private ForceMove robot;   // �͂�������Ώ�
    [SerializeField] private PartsPerformanceData partsPerformanceData;

    // ���̎��_�ł̑��d�ʂ��v�Z���ĕԂ�
    public float GetAllWeight()
    {
        float allWeight = 0f;
        var datas = PartsInfo.Instance.GetPartsList();
        datas.ForEach(data => allWeight += partsPerformanceData.getData(data.id).m);
        return allWeight;
    }

    [ContextMenu("Debug/UseParts")]
    void UseParts()
    {
        // �g�p����p�[�c�̃f�[�^���擾����
        // �i�g�p�����A�C�e���̏����͗͂���������s���j
        PartsInfo PI = PartsInfo.Instance;
        if (PI.Length == 0) throw new Exception("�g�p����p�[�c������܂���B");
        var data = PI.GetParts(0);
        var performance = partsPerformanceData.getData(data.id);

        // ������͂��\�z����
        IForce force;
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

        // �\�z�����͂�������
        robot.AddForce(force);
    }

    // �f�o�b�O�p�B�g�p�p�[�c���X�g�Ƀe�X�g�p�[�c��ǉ�����B
    [ContextMenu("Debug/GetTestParts")]
    private void AddTestParts()
    {
        var data = new PartsInfo.PartsData();
        data.id = PartsPerformance.E_PartsID.TestParts;
        data.angle = 80;
        PartsInfo.Instance.AddParts(data);
    }
}
