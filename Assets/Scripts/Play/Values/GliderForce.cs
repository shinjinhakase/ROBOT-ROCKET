using System;
using UnityEngine;

[Serializable]
public class GliderForce : IForce
{
    // ・グライダーの角度を傾けない場合の軌道予測
    // 速度はグライダーに沿う向きで安定する。
    // グライダーが上向き→左に進んでいた場合左に減速しながら上昇するが、失速すると重力によって左に逸れていく。
    // グライダーが下向き→右に加速しながら、グライダー方向に安定して落下する。

    // 基本抗力は揚力より弱め。
    private bool IsPartsForce;
    public float m;     // （アイテムの重量）
    public float Angle; // グライダーの角度（-90度～+90度）
    public float F;     // 揚力を微調整するための係数
    public float t;     // 力を加える時間（秒）
    public float R;     // 抗力を微調整するための係数

    private Vector2 Fe1;    // 前方向の基底ベクトル
    private Vector2 Fe2;    // 下方向の基底ベクトル

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

        // 角度を適正な方向に調整する（進行方向を向かせる）
        float angle360 = Angle % 360;
        if (angle360 < 0) angle360 += 360;
        if (angle360 > 90 && angle360 < 270)
        {
            this.Angle -= 180;
            Debug.LogWarning("グライダーの角度の初期設定が進行方向を向いていないので修正しました。");
        }

        // 計算に使う値を先に計算する
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

    // 速度を前方と下方に分解する
    private void CalcVelocity(Vector2 velocity, out float vForward, out float vBelow)
    {
        vForward = Vector2.Dot(Fe1, velocity);
        vBelow = Vector2.Dot(Fe2, velocity);
    }
}
