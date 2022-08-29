using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseForce : IForce
{
    public float Angle; // 角度
    public float F;     // 力の大きさ

    private Vector2 Force;     // 加える力（ベクトル）

    public ImpulseForce(float Angle, float F)
    {
        this.Angle = Angle;
        this.F = F;

        // 計算に使う値を先に計算する
        float radAngle = Angle * Mathf.Deg2Rad;
        Force = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)) * F / Time.fixedDeltaTime;
    }

    Vector2 IForce.CalcForce(Vector2 nowForce, Vector2 velocity) => Force;

    bool IForce.IsEnd() => true;

    void IForce.StartPush() { }
}
