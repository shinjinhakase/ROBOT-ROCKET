using UnityEngine;

// ロケットやプロペラなど、同じ方向に持続的に力を加え続ける力のクラス
public class PressForce : IForce
{
    public float Angle; // 角度
    public float F;     // 力の大きさ
    public float t;     // 力を加える時間（秒）
    public float k;     // 抵抗力（大きい程終端速度が遅くなる）

    private int cntFrame;   // 経過時間（フレーム）
    private int endFrame;   // 終了時間（フレーム）
    private Vector2 Fe;     // 前方向の基底ベクトル

    public PressForce(float Angle, float F, float t, float k)
    {
        this.Angle = Angle;
        this.F = F;
        this.t = t;
        this.k = k;

        // 計算に使う値を先に計算する
        float radAngle = Angle * Mathf.Deg2Rad;
        Fe = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));

        endFrame = Mathf.FloorToInt(t / Time.fixedDeltaTime);
    }

    Vector2 IForce.CalcForce(Vector2 nowForce, Vector2 velocity) => Fe * (F - k * CalcFrontVelocity(velocity));

    bool IForce.IsEnd() => cntFrame++ == endFrame;

    void IForce.StartPush() => cntFrame = 0;

    // 前方向の速度を計算する（読まなくて良いけど解説↓）
    //      （Feを前方向の基底ベクトル、Fe2を下方向の基底ベクトルとする）
    //      実数s, tについて [Fe Fe2][s t] = [velocity]とすると、[s t] = [Fe Fe2]^-1 ･ [velocity]
    //      [Fe Fe2]の逆行列は元の形と同じ形である。かつ対称行列なので、元のベクトルを流用して計算できる。
    //      このとき、sの値が前方方向の速度と等しいので、Fe ･ velocityを計算する。
    private float CalcFrontVelocity(Vector2 velocity) => Vector2.Dot(Fe, velocity);
}
