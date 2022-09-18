using UnityEngine;

// �͂̃C���^�[�t�F�[�X
public interface IForce
{
    public bool IsMainRobot { get; set; }
    public int frameCnt { get; protected set; }

    // �����_�ł̍��͂ƌ��݂̑��x����A�͂��v�Z���郁�\�b�h
    public Vector2 CalcForce(Vector2 nowForce, Vector2 velocity);

    // �͂�������̂��~�߂邩�𔻒肷�郁�\�b�h�i���t���[���P�񂸂Ăяo����邱�Ƃɂ���j
    public bool IsEnd();

    // �͂������n�߂��ۂɌĂяo����郁�\�b�h
    public void StartPush();

    // �͂������Ȃ����ۂɃv���C���[����������d�ʁi�A�C�e�����g�p�����ۂ̗͂Ŏg�p�j
    public float GetMass();

    // �͂������Ȃ����ۂɌĂ΂�鏈���i��ɃA�C�e���g�p���t���O�̐ؑցj
    public void EndPress();
}
