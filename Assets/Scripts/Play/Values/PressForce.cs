using System;
using UnityEngine;

// ロケットやプロペラなど、同じ方向に持続的に力を加え続ける力のクラス
[Serializable]
public class PressForce : IForce
{
    private bool IsPartsForce;
    public float m;     // （アイテムの重量）
    public float Angle; // 角度
    public float F;     // 力の大きさ
    public float t;     // 力を加える時間（秒）
    public float k;     // 抵抗力（大きい程終端速度が遅くなる）

    protected int cntFrame; // 経過時間（フレーム）
    private int endFrame;   // 終了時間（フレーム）
    private Vector2 Fe;     // 前方向の基底ベクトル

    private PlayPartsManager playPartsManager;

    public PressForce(float Angle, float F, float t, float k, float m = 0, bool IsPartsForce = false)
    {
        this.IsPartsForce = IsPartsForce;
        this.m = m;
        this.Angle = Angle;
        this.F = F;
        this.t = t;
        this.k = k;

        // 計算に使う値を先に計算する
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

    // 前方向の速度を計算する（読まなくて良いけど解説↓）
    //      （Feを前方向の基底ベクトル、Fe2を下方向の基底ベクトルとする）
    //      実数s, tについて [Fe Fe2][s t] = [velocity]とすると、[s t] = [Fe Fe2]^-1 ･ [velocity]
    //      [Fe Fe2]の逆行列は元の形と同じ形である。かつ対称行列なので、元のベクトルを流用して計算できる。
    //      このとき、sの値が前方方向の速度と等しいので、Fe ･ velocityを計算する。
    private float CalcFrontVelocity(Vector2 velocity) => Vector2.Dot(Fe, velocity);
}
