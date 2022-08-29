using UnityEngine;

// 力のインターフェース
public interface IForce
{
    // 現時点での合力と現在の速度から、力を計算するメソッド
    public Vector2 CalcForce(Vector2 nowForce, Vector2 velocity);

    // 力を加えるのを止めるかを判定するメソッド（毎フレーム１回ずつ呼び出されることにする）
    public bool IsEnd();

    // 力が加わり始めた際に呼び出されるメソッド
    public void StartPush();
}
