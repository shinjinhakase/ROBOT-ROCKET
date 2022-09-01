using System;

// 対象が指定のオブジェクトと重なっている間、押し続ける力のクラス
// （力の計算はロケットとかプロペラとかと同じ式にする）
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
