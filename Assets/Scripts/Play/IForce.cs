using UnityEngine;

// �͂̃C���^�[�t�F�[�X
public interface IForce
{
    // �����_�ł̍��͂ƌ��݂̑��x����A�͂��v�Z���郁�\�b�h
    public Vector2 CalcForce(Vector2 nowForce, Vector2 velocity);

    // �͂�������̂��~�߂邩�𔻒肷�郁�\�b�h�i���t���[���P�񂸂Ăяo����邱�Ƃɂ���j
    public bool IsEnd();

    // �͂������n�߂��ۂɌĂяo����郁�\�b�h
    public void StartPush();
}
