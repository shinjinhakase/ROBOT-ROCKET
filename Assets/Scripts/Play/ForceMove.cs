using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �͂ɂ���ē����𐧌䂷��Component
[RequireComponent(typeof(Rigidbody2D))]
public class ForceMove : MonoBehaviour
{
    // ���������Ă���͂̃��X�g
    [SerializeReference]
    private List<IForce> forces = new List<IForce>();

    private Rigidbody2D rb;

    [Header("�e�X�g�p�p�����[�^")]
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
        // �͂��v�Z���ĉ�����
        Vector2 F = CalcForce();
        rb.AddForce(F);

        // �����Ȃ����͂����X�g���珜������
        forces.RemoveAll(force => force.IsEnd());
    }

    // �f�o�b�O�p�B���炩�̗͂�ǉ�����
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

    // ���X�g�ɗ͂�ǉ�����
    public void AddForce(IForce force)
    {
        force.StartPush();
        forces.Add(force);
    }


    // �����鍇�͂̌v�Z
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
