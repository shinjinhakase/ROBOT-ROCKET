using System;

// �Ώۂ��w��̃I�u�W�F�N�g�Əd�Ȃ��Ă���ԁA����������͂̃N���X
// �i�͂̌v�Z�̓��P�b�g�Ƃ��v���y���Ƃ��Ɠ������ɂ���j
[Serializable]
public class CollisionForce : PressForce, IForce
{
    private ForceGimick gimick;
    private ForceMove target;

    public CollisionForce(ForceGimick gimick, ForceMove target, float Angle, float F, float k) : base(Angle, F, 0, k, 0, false)
    {
        this.gimick = gimick;
        this.target = target;
    }

    public bool IsEnd() => !gimick.CheckCollision(target);
}
