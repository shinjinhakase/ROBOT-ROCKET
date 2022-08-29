using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 力によって動きを制御するComponent
[RequireComponent(typeof(Rigidbody2D))]
public class ForceMove : MonoBehaviour
{
    // 今加えられている力のリスト
    [SerializeReference]
    private List<IForce> forces = new List<IForce>();

    private Rigidbody2D rb;

    [Header("テスト用パラメータ")]
    [SerializeField] private float testAngle;
    [SerializeField] private float testF;
    [SerializeField] private float testT;
    [SerializeField] private float testK;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 力を計算して加える
        Vector2 F = CalcForce();
        rb.AddForce(F);

        // 無くなった力をリストから除去する
        forces.RemoveAll(force => force.IsEnd());
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

    // リストに力を追加する
    public void AddForce(IForce force)
    {
        force.StartPush();
        forces.Add(force);
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
}
