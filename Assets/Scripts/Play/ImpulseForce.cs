using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseForce : IForce
{
    public float Angle; // �p�x
    public float F;     // �͂̑傫��

    private Vector2 Force;     // ������́i�x�N�g���j

    public ImpulseForce(float Angle, float F)
    {
        this.Angle = Angle;
        this.F = F;

        // �v�Z�Ɏg���l���Ɍv�Z����
        float radAngle = Angle * Mathf.Deg2Rad;
        Force = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)) * F / Time.fixedDeltaTime;
    }

    Vector2 IForce.CalcForce(Vector2 nowForce, Vector2 velocity) => Force;

    bool IForce.IsEnd() => true;

    void IForce.StartPush() { }
}
