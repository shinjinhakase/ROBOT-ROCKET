using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Parts", menuName = "ScriptableObjects/PartsPerformance")]
public class PartsPerformance : ScriptableObject
{
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
    public enum E_PartsID
    {
        TestParts
    }

    public E_PartsID id;
    public E_ForceType forceType;
    public float m; // ����
    public float F; // ��
    public float t; // ����
    public float R; // ��R��
    public float k; // �W��
    public float cooltime;  // ���ʏI����̃N�[���^�C��
}
