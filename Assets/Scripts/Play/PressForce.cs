using UnityEngine;

// ���P�b�g��v���y���ȂǁA���������Ɏ����I�ɗ͂�����������͂̃N���X
public class PressForce : IForce
{
    public float Angle; // �p�x
    public float F;     // �͂̑傫��
    public float t;     // �͂������鎞�ԁi�b�j
    public float k;     // ��R�́i�傫�����I�[���x���x���Ȃ�j

    private int cntFrame;   // �o�ߎ��ԁi�t���[���j
    private int endFrame;   // �I�����ԁi�t���[���j
    private Vector2 Fe;     // �O�����̊��x�N�g��

    public PressForce(float Angle, float F, float t, float k)
    {
        this.Angle = Angle;
        this.F = F;
        this.t = t;
        this.k = k;

        // �v�Z�Ɏg���l���Ɍv�Z����
        float radAngle = Angle * Mathf.Deg2Rad;
        Fe = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));

        endFrame = Mathf.FloorToInt(t / Time.fixedDeltaTime);
    }

    Vector2 IForce.CalcForce(Vector2 nowForce, Vector2 velocity) => Fe * (F - k * CalcFrontVelocity(velocity));

    bool IForce.IsEnd() => cntFrame++ == endFrame;

    void IForce.StartPush() => cntFrame = 0;

    // �O�����̑��x���v�Z����i�ǂ܂Ȃ��ėǂ����ǉ�����j
    //      �iFe��O�����̊��x�N�g���AFe2���������̊��x�N�g���Ƃ���j
    //      ����s, t�ɂ��� [Fe Fe2][s t] = [velocity]�Ƃ���ƁA[s t] = [Fe Fe2]^-1 � [velocity]
    //      [Fe Fe2]�̋t�s��͌��̌`�Ɠ����`�ł���B���Ώ̍s��Ȃ̂ŁA���̃x�N�g���𗬗p���Čv�Z�ł���B
    //      ���̂Ƃ��As�̒l���O�������̑��x�Ɠ������̂ŁAFe � velocity���v�Z����B
    private float CalcFrontVelocity(Vector2 velocity) => Vector2.Dot(Fe, velocity);
}