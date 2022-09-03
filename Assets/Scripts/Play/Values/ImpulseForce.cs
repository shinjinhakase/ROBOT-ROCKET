using System;
using UnityEngine;

[Serializable]
public class ImpulseForce : IForce
{
    private bool IsPartsForce;   // アイテム産の力かのフラグ
    public float m;     // （アイテムの質量）
    public float Angle; // 角度
    public float F;     // 力の大きさ

    private Vector2 Force;     // 加える力（ベクトル）

    public ImpulseForce(float Angle, float F, float m = 0, bool IsPartsForce = false)
    {
        this.IsPartsForce = IsPartsForce;
        this.m = m;
        this.Angle = Angle;
        this.F = F;

        // 計算に使う値を先に計算する
        float radAngle = Angle * Mathf.Deg2Rad;
        Force = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)) * F / Time.fixedDeltaTime;
    }

    private bool _isMainRobot = false;
    bool IForce.IsMainRobot { get { return _isMainRobot; } set { _isMainRobot = value; } }

    Vector2 IForce.CalcForce(Vector2 nowForce, Vector2 velocity) => Force;

    bool IForce.IsEnd() => true;

    void IForce.StartPush() { }

    float IForce.GetMass() => m;

    void IForce.EndPress()
    {
        if (IsPartsForce) PlayPartsManager.Instance.IsUsingParts = false;
        if (_isMainRobot) ReplayInputManager.Instance.SetForce(this);
    }
}
