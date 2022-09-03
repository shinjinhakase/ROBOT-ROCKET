using System;
using UnityEngine;

[Serializable]
public class GliderForce : IForce
{
    // �E�O���C�_�[�̊p�x���X���Ȃ��ꍇ�̋O���\��
    // ���x�̓O���C�_�[�ɉ��������ň��肷��B
    // �O���C�_�[������������ɐi��ł����ꍇ���Ɍ������Ȃ���㏸���邪�A��������Əd�͂ɂ���č��Ɉ��Ă����B
    // �O���C�_�[�����������E�ɉ������Ȃ���A�O���C�_�[�����Ɉ��肵�ė�������B

    // ��{�R�͂͗g�͂���߁B
    private bool IsPartsForce;
    public float m;     // �i�A�C�e���̏d�ʁj
    public float Angle; // �O���C�_�[�̊p�x�i-90�x�`+90�x�j
    public float F;     // �g�͂���������邽�߂̌W��
    public float t;     // �͂������鎞�ԁi�b�j
    public float R;     // �R�͂���������邽�߂̌W��

    private Vector2 Fe1;    // �O�����̊��x�N�g��
    private Vector2 Fe2;    // �������̊��x�N�g��

    private int cntFrame;
    private int endFrame;

    private PlayPartsManager playPartsManager;

    public GliderForce(float Angle, float F, float t, float R, float m = 0, bool IsPartsForce = false)
    {
        this.IsPartsForce = IsPartsForce;
        this.m = m;
        this.Angle = Angle;
        this.F = F;
        this.t = t;
        this.R = R;

        // �p�x��K���ȕ����ɒ�������i�i�s��������������j
        float angle360 = Angle % 360;
        if (angle360 < 0) angle360 += 360;
        if (angle360 > 90 && angle360 < 270)
        {
            this.Angle -= 180;
            Debug.LogWarning("�O���C�_�[�̊p�x�̏����ݒ肪�i�s�����������Ă��Ȃ��̂ŏC�����܂����B");
        }

        // �v�Z�Ɏg���l���Ɍv�Z����
        float radAngle = Angle * Mathf.Deg2Rad;
        float angleSin = Mathf.Sin(radAngle);
        float angleCos = Mathf.Cos(radAngle);
        Fe1 = new Vector2(angleCos, angleSin);
        Fe2 = new Vector2(angleSin, -angleCos);

        playPartsManager = PlayPartsManager.Instance;
        endFrame = Mathf.RoundToInt(t / Time.fixedDeltaTime);
    }

    Vector2 IForce.CalcForce(Vector2 nowForce, Vector2 velocity)
    {
        CalcVelocity(velocity, out float vForward, out float vBelow);
        return nowForce - Fe1 * vForward * R - Fe2 * vBelow * F;
    }

    bool IForce.IsEnd() => cntFrame++ == endFrame || (IsPartsForce && !playPartsManager.IsUsingParts);

    void IForce.StartPush() => cntFrame = 0;

    float IForce.GetMass() => m;

    void IForce.EndPress()
    {
        if (IsPartsForce) PlayPartsManager.Instance.IsUsingParts = false;
    }

    // ���x��O���Ɖ����ɕ�������
    private void CalcVelocity(Vector2 velocity, out float vForward, out float vBelow)
    {
        vForward = Vector2.Dot(Fe1, velocity);
        vBelow = Vector2.Dot(Fe2, velocity);
    }
}
