using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects")]
public class PartsPerformanceData : ScriptableObject
{
    public List<RobotPerformance> dataList = new List<RobotPerformance>();

    public RobotPerformance getPartsPerformance(E_PartsID id)
    {
        foreach(RobotPerformance performance in dataList)
        {
            if (performance.id == id) return performance;
        }
        throw new Exception("�p�[�cID:" + id.ToString() + "�̐��\�͒�`����Ă��܂���B");
    }

    // �͂̉��������`����񋓌^
    public enum E_ForceType
    {
        Bomb,           //���e�B�u�ԓI�ȗ͂�������
        Rocket,         //���P�b�g�B�����I�Œ�����E�����x�ȗ͂�������B
        Propeller,      //�v���y���B�����I�ō������E�ᑬ�x�ȗ͂�������B
        Glider,         //�O���C�_�[�B����s�@�Ȃǂ̂悤�ȓ����I�ȗ͂�������B
        CollisionForce  //����(�X�e�[�W�M�~�b�N�Ȃ�)�B�v���C���[���Փ˂��Ă���ԁA�͂�����������B
    }
    // �A�C�e������ʂ���ID�̖������ʂ����񋓌^
    public enum E_PartsID {
        TestParts
    }

    [Serializable]
    public class RobotPerformance
    {
        public E_PartsID id;
        public E_ForceType forceType;
        public float m; // ����
        public float F; // ��
        public float t; // ����
        public float v; // �I�[���x
        public float R; // ��R��
        public float k; // �W��
    }
}
