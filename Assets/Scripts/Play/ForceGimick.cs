using System.Collections.Generic;
using UnityEngine;

// �ΏۂƏd�Ȃ��Ă���ԁA��(CollisionForce)��������M�~�b�N
public class ForceGimick : MonoBehaviour
{
    // �͂������Ă���Ώۂ̃��X�g
    private List<ForceMove> targets = new List<ForceMove>();

    // ������͂̐ݒ�
    [SerializeField] private float Angle;   // �p�x
    [SerializeField] private float F;       // �͂̑傫��
    [SerializeField] private float k;       // �R�͂̌W��

    // ���̃I�u�W�F�N�g�ƏՓ˂����ۂɌĂяo����郁�\�b�h
    public void StartPush(Collider2D collision)
    {
        // �͂���������I�u�W�F�N�g������
        if (!collision.TryGetComponent(out ForceMove move)) return;
        if (targets.Contains(move)) return;

        // �͂������A�Ǘ����X�g�ɒǉ�����
        targets.Add(move);
        IForce force = new CollisionForce(this, move, Angle, F, k);
        move.AddForce(force);
    }

    // ���̃I�u�W�F�N�g���痣�ꂽ�ۂɌĂяo����郁�\�b�h
    public void EndPush(Collider2D collision)
    {
        // �͂���������I�u�W�F�N�g������
        if (!collision.TryGetComponent(out ForceMove move)) return;

        // �Ώۂ��Ǘ����珜�O����
        if(!targets.Contains(move)) return;
        targets.Remove(move);
    }

    // �w��̑Ώۂ��d�Ȃ��Ă��邩���肷�郁�\�b�h
    public bool CheckCollision(ForceMove target) => targets.Contains(target);
}
