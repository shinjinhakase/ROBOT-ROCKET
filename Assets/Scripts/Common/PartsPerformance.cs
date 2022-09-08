using System;
using System.Collections.Generic;
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
        CollisionForce, //����(�X�e�[�W�M�~�b�N�Ȃ�)�B�v���C���[���Փ˂��Ă���ԁA�͂�����������B
        NoForce         //�U���p�̃A�C�e���ȂǁB���ɗ͂������Ȃ��B
    }
    // �A�C�e������ʂ���ID�̖������ʂ����񋓌^
    public enum E_PartsID
    {
        TestParts,
        Rocket1,
        Rocket2,
        Rocket3
    }

    // �������v���C�V�[���֘A
    public E_PartsID id;
    public E_ForceType forceType;
    public float m; // ����
    public float F; // ��
    public float t; // ����
    public float R; // ��R��
    public float k; // �W��
    public float cooltime;  // ���ʏI����̃N�[���^�C��
    public List<SummonableObject> summonObjects = new List<SummonableObject>(); //�����I�u�W�F�N�g���X�g

    // UI�֘A����
    public string partsName;    //�A�C�e�����O
    [Tooltip("�������ɕ\������錩���ځB")]
    public Sprite partsSprite;
    [Tooltip("UI�p�ɃA�C�R���Ƃ��ėp�����錩���ځB")]
    public Sprite iconSprite;
    [Tooltip("�������BUI�Ńp�[�c�����ɗp���܂��B"), TextArea(3, 5)]
    public string description;  //�A�C�e������
}
