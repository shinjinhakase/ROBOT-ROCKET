using System;

// 対象が指定のオブジェクトと重なっている間、押し続ける力のクラス
// （力の計算はロケットとかプロペラとかと同じ式にする）
[Serializable]
public class CollisionForce : PressForce, IForce
{
    private ForceGimick gimick;
    private ForceMove target;

    public CollisionForce(ForceGimick gimick, ForceMove target,
        float Angle, float F, float k, float m = 0, bool IsPartsForce = false) : base(Angle, F, 0, k, m, IsPartsForce)
    {
        this.gimick = gimick;
        this.target = target;
    }

    public bool IsEnd()
    {
        cntFrame++;
        return !gimick.CheckCollision(target);
    }
}
