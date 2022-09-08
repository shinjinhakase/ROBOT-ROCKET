using System;

// �Ώۂ��w��̃I�u�W�F�N�g�Əd�Ȃ��Ă���ԁA����������͂̃N���X
// �i�͂̌v�Z�̓��P�b�g�Ƃ��v���y���Ƃ��Ɠ������ɂ���j
[Serializable]
public class CollisionForce : PressForce, IForce
{
    private ForceGimick gimick;
    private ForceMove target;

    public CollisionForce(ForceGimick gimick, ForceMove target,
        float Angle, float F, float k, float m = 0, bool IsPartsForce = false) : base(Angle, F, 0, k, m, IsPartsForce)
    {
        this.gimick = gimick;
        this.target = target;
    }

    public bool IsEnd()
    {
        cntFrame++;
        return !gimick.CheckCollision(target);
    }
}
