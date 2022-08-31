using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �j��\�ȃI�u�W�F�N�g�̓�����`����Component�B
[RequireComponent(typeof(Collider2D))]
public class CrashableObject : MonoBehaviour
{
    [Tooltip("�j�󂳂�Ă��犮�S�ɃI�u�W�F�N�g��������܂ł̎���")]
    [SerializeField] private float CrashDuration = 0f;

    private Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    // �j�󎞂ɌĂяo����郁�\�b�h
    [ContextMenu("Debug/Crash")]
    private void Crash()
    {
        // �����蔻��𖳌���
        _collider.enabled = false;

        // �w��b����Ɏ��g��j�󂷂�i�A�j���[�V�����p�̑ҋ@���ԁj
        Destroy(gameObject, CrashDuration);
    }
}
