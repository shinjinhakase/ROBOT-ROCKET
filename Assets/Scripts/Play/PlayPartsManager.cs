using System;
using UnityEngine;
using UnityEngine.Events;

// �v���C�V�[���Ŏg�p����A�C�e�����Ǘ�����Component�B
// �����ł̃A�C�e���̓���E�g�p�A�d�ʌv�Z�͂����������B
public class PlayPartsManager : SingletonMonoBehaviourInSceneBase<PlayPartsManager>
{
    // ���݃p�[�c�g�p�����̃t���O
    [NonSerialized] public bool IsUsingParts = false;

    private PartsInfo partsInfo;
    [SerializeField] private PartsPerformanceData partsPerformanceData;

    [SerializeField] private UnityEvent<PartsInfo.PartsData> getPartsEvent = new UnityEvent<PartsInfo.PartsData>();

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

    // �p�[�cID����A�C�e�����\��Ԃ�
    public PartsPerformance GetPerformance(PartsPerformance.E_PartsID id) => partsPerformanceData.getData(id);

    // �p�[�c���g���i�g���p�[�c�̃f�[�^�ƁA���܂��͂�Ԃ��j
    public void UseParts(out PartsPerformance performance, out PartsInfo.PartsData data, out IForce force)
    {
        // �g�p����p�[�c�̃f�[�^���擾����
        if (partsInfo.Length == 0) throw new Exception("�g�p����p�[�c������܂���B");
        IsUsingParts = true;
        data = partsInfo.GetParts(0);
        performance = partsPerformanceData.getData(data.id);

        // ������͂��\�z����
        switch (performance.forceType)
        {
            case PartsPerformance.E_ForceType.Bomb:
                force = new ImpulseForce(data.angle, performance.F, performance.m, true);
                break;
            case PartsPerformance.E_ForceType.Rocket:
            case PartsPerformance.E_ForceType.Propeller:
                force = new PressForce(data.angle, performance.F, performance.t, performance.k, performance.m, true);
                break;
            case PartsPerformance.E_ForceType.NoForce:
                force = null;
                break;
            case PartsPerformance.E_ForceType.Glider:
                force = new GliderForce(data.angle, performance.F, performance.t, performance.R, performance.m, true);
                break;
            case PartsPerformance.E_ForceType.CollisionForce:
            default:
                throw new Exception("���{�b�g�ɉ�����͂��\�z�ł��܂���B");
        }
    }

    // �p�[�c���l�����鏈��
    public void GetParts(PartsInfo.PartsData data, out PartsPerformance performance)
    {
        partsInfo.AddParts(data);
        performance = partsPerformanceData.getData(data.id);
        getPartsEvent.Invoke(data);

        Debug.Log("�p�[�c���l�����܂����FID = " + data.id);
    }
    public void GetParts(PartsInfo.PartsData data)
    {
        GetParts(data, out _);
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

    // �p�[�c�̎g�p�󋵂����Z�b�g����B
    public void ResetPartsStatus()
    {
        // �A�C�e���̎g�p�󋵂����Z�b�g���A�J�X�^���p�[�c���X�g��ۑ�������Ԃɖ߂��B
        IsUsingParts = false;
        partsInfo.Reset();
        partsInfo = PartsInfo.Instance;

        Debug.Log("�J�X�^���p�[�c�f�[�^��ۑ����̏�ԂɃ��Z�b�g���܂���");
    }
}
