using System;
using UnityEngine;

// ���P�b�g��v���y���ȂǁA���������Ɏ����I�ɗ͂�����������͂̃N���X
[Serializable]
public class PressForce : IForce
{
    private bool IsPartsForce;
    public float m;     // �i�A�C�e���̏d�ʁj
    public float Angle; // �p�x
    public float F;     // �͂̑傫��
    public float t;     // �͂������鎞�ԁi�b�j
    public float k;     // ��R�́i�傫�����I�[���x���x���Ȃ�j

    protected int cntFrame; // �o�ߎ��ԁi�t���[���j
    private int endFrame;   // �I�����ԁi�t���[���j
    private Vector2 Fe;     // �O�����̊��x�N�g��

    private PlayPartsManager playPartsManager;

    public PressForce(float Angle, float F, float t, float k, float m = 0, bool IsPartsForce = false)
    {
        this.IsPartsForce = IsPartsForce;
        this.m = m;
        this.Angle = Angle;
        this.F = F;
        this.t = t;
        this.k = k;

        // �v�Z�Ɏg���l���Ɍv�Z����
        float radAngle = Angle * Mathf.Deg2Rad;
        Fe = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));

        playPartsManager = PlayPartsManager.Instance;
        endFrame = Mathf.RoundToInt(t / Time.fixedDeltaTime);
    }

    private bool _isMainRobot = false;
    bool IForce.IsMainRobot { get { return _isMainRobot; } set { _isMainRobot = value; } }
    int IForce.frameCnt { get { return cntFrame; } set { cntFrame = value; } }

    Vector2 IForce.CalcForce(Vector2 nowForce, Vector2 velocity) => Fe * (F - k * CalcFrontVelocity(velocity));

    bool IForce.IsEnd() => cntFrame++ == endFrame || (IsPartsForce && !playPartsManager.IsUsingParts);

    void IForce.StartPush() => cntFrame = 0;

    float IForce.GetMass() => m;

    void IForce.EndPress()
    {
        if (IsPartsForce) PlayPartsManager.Instance.IsUsingParts = false;
        if (_isMainRobot) ReplayInputManager.Instance.SetForce(this);
    }

    // �O�����̑��x���v�Z����i�ǂ܂Ȃ��ėǂ����ǉ�����j
    //      �iFe��O�����̊��x�N�g���AFe2���������̊��x�N�g���Ƃ���j
    //      ����s, t�ɂ��� [Fe Fe2][s t] = [velocity]�Ƃ���ƁA[s t] = [Fe Fe2]^-1 � [velocity]
    //      [Fe Fe2]�̋t�s��͌��̌`�Ɠ����`�ł���B���Ώ̍s��Ȃ̂ŁA���̃x�N�g���𗬗p���Čv�Z�ł���B
    //      ���̂Ƃ��As�̒l���O�������̑��x�Ɠ������̂ŁAFe � velocity���v�Z����B
    private float CalcFrontVelocity(Vector2 velocity) => Vector2.Dot(Fe, velocity);
}
