using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 力によって動きを制御するComponent
[RequireComponent(typeof(Rigidbody2D))]
public class ForceMove : MonoBehaviour
{
    public static float RobotWeight => 1f;  // ロボット単体の重量

    // 今加えられている力のリスト
    [SerializeReference]
    private List<IForce> forces = new List<IForce>();

    [Header("テスト用パラメータ")]
    [SerializeField] private float testAngle;
    [SerializeField] private float testF;
    [SerializeField] private float testT;
    [SerializeField] private float testK;

    private Rigidbody2D rb;
    private Vector3 firstPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        firstPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // 力を計算して加える
        Vector2 F = CalcForce();
        rb.AddForce(F);

        // 無くなった力をリストから除去する
        for(int i = 0; i < forces.Count; i++)
        {
            if (forces[i].IsEnd())
            {
                // 力が無くなったら重量をプレイヤーから引く（アイテム産の力であれば重量が設定されている）
                forces[i].EndPress();
                rb.mass = rb.mass - forces[i].GetMass();
                forces.RemoveAt(i);
                i--;
            }
        }
    }

    // デバッグ用。何らかの力を追加する
    [ContextMenu("Debug/AddPressForce")]
    private void AddTestPressForce()
    {
        PressForce f = new PressForce(testAngle, testF, testT, testK);
        AddForce(f);
    }
    [ContextMenu("Debug/AddImpulseForce")]
    private void AddTestImpulseForce()
    {
        ImpulseForce f = new ImpulseForce(testAngle, testF);
        AddForce(f);
    }
    [ContextMenu("Debug/AddGliderForce")]
    private void AddGliderForce()
    {
        GliderForce f = new GliderForce(testAngle, testF, testT, testK);
        AddForce(f);
    }

    // リストに力を追加する
    public void AddForce(IForce force)
    {
        if (force == null) return;
        force.StartPush();
        forces.Add(force);
    }

    // 重量を設定する（初期重量の設定用。アイテムを使用した後は自動で重量が減っていく）
    public void SetWeight(float mass) {
        rb.mass = mass;
    }

    // 加える合力の計算
    private Vector2 CalcForce()
    {
        Vector2 F = Vector2.zero;
        Vector2 v = rb.velocity;
        foreach(IForce force in forces)
        {
            F = force.CalcForce(F, v);
        }
        return F;
    }

    // 加える力を無くす
    public void ZeroForce()
    {
        forces.Clear();
    }

    // 初期状態にリセットする
    public void ResetToFirst()
    {
        ZeroForce();
        rb.velocity = Vector2.zero;
        transform.position = firstPosition;
    }
}
