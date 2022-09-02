using System.Collections.Generic;
using UnityEngine;

// 対象と重なっている間、力(CollisionForce)を加えるギミック
public class ForceGimick : MonoBehaviour
{
    // 力を加えている対象のリスト
    private List<ForceMove> targets = new List<ForceMove>();

    // 加える力の設定
    [SerializeField] private float Angle;   // 角度
    [SerializeField] private float F;       // 力の大きさ
    [SerializeField] private float k;       // 抗力の係数

    // 他のオブジェクトと衝突した際に呼び出されるメソッド
    public void StartPush(Collider2D collision)
    {
        // 力を加えられるオブジェクトか判定
        if (!collision.TryGetComponent(out ForceMove move)) return;
        if (targets.Contains(move)) return;

        // 力を加え、管理リストに追加する
        targets.Add(move);
        IForce force = new CollisionForce(this, move, Angle, F, k);
        move.AddForce(force);
    }

    // 他のオブジェクトから離れた際に呼び出されるメソッド
    public void EndPush(Collider2D collision)
    {
        // 力を加えられるオブジェクトか判定
        if (!collision.TryGetComponent(out ForceMove move)) return;

        // 対象を管理から除外する
        if(!targets.Contains(move)) return;
        targets.Remove(move);
    }

    // 指定の対象が重なっているか判定するメソッド
    public bool CheckCollision(ForceMove target) => targets.Contains(target);
}
