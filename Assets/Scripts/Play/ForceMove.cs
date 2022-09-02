using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �͂ɂ���ē����𐧌䂷��Component
[RequireComponent(typeof(Rigidbody2D))]
public class ForceMove : MonoBehaviour
{
    public static float RobotWeight => 1f;  // ���{�b�g�P�̂̏d��

    // ���������Ă���͂̃��X�g
    [SerializeReference]
    private List<IForce> forces = new List<IForce>();

    [Header("�e�X�g�p�p�����[�^")]
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
        // �͂��v�Z���ĉ�����
        Vector2 F = CalcForce();
        rb.AddForce(F);

        // �����Ȃ����͂����X�g���珜������
        for(int i = 0; i < forces.Count; i++)
        {
            if (forces[i].IsEnd())
            {
                // �͂������Ȃ�����d�ʂ��v���C���[��������i�A�C�e���Y�̗͂ł���Ώd�ʂ��ݒ肳��Ă���j
                forces[i].EndPress();
                rb.mass = rb.mass - forces[i].GetMass();
                forces.RemoveAt(i);
                i--;
            }
        }
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
    [ContextMenu("Debug/AddGliderForce")]
    private void AddGliderForce()
    {
        GliderForce f = new GliderForce(testAngle, testF, testT, testK);
        AddForce(f);
    }

    // ���X�g�ɗ͂�ǉ�����
    public void AddForce(IForce force)
    {
        if (force == null) return;
        force.StartPush();
        forces.Add(force);
    }

    // �d�ʂ�ݒ肷��i�����d�ʂ̐ݒ�p�B�A�C�e�����g�p������͎����ŏd�ʂ������Ă����j
    public void SetWeight(float mass) {
        rb.mass = mass;
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

    // ������͂𖳂���
    public void ZeroForce()
    {
        forces.Clear();
    }

    // ������ԂɃ��Z�b�g����
    public void ResetToFirst()
    {
        ZeroForce();
        rb.velocity = Vector2.zero;
        transform.position = firstPosition;
    }
}
