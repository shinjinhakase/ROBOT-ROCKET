using UnityEngine;

// 力のインターフェース
public interface IForce
{
    public bool IsMainRobot { get; set; }
    public int frameCnt { get; protected set; }

    // 現時点での合力と現在の速度から、力を計算するメソッド
    public Vector2 CalcForce(Vector2 nowForce, Vector2 velocity);

    // 力を加えるのを止めるかを判定するメソッド（毎フレーム１回ずつ呼び出されることにする）
    public bool IsEnd();

    // 力が加わり始めた際に呼び出されるメソッド
    public void StartPush();

    // 力が無くなった際にプレイヤーから引かれる重量（アイテムを使用した際の力で使用）
    public float GetMass();

    // 力が無くなった際に呼ばれる処理（主にアイテム使用中フラグの切替）
    public void EndPress();
}
