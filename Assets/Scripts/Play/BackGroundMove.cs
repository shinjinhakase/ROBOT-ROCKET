using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    [SerializeField] private Transform _target; // �Ǐ]�Ώہi��{�J�����j

    [Header("�ړ��ݒ�")]
    [Tooltip("��n�_�i��{�̓X�^�[�g�n�_�ɂȂ邩�ȁH�j")]
    [SerializeField] private float _homePosition = 0;
    [Tooltip("��n�_�ł�X���W�̈ʒu")]
    [SerializeField] private float InitXPosition;
    [Tooltip("�������ɓ�������E����")]
    [SerializeField] private float MinXLocate;
    [Tooltip("�E�����ɓ�������E����")]
    [SerializeField] private float MaxXLocate;

    [Tooltip("�������̈ړ����x�i1�ŒǏ]�Ώۂ̈ړ��Ɠ����j")]
    [SerializeField] private float XVelocity;

    private Vector3 _homeVector;

    private void Start()
    {
        _homeVector = transform.position;
    }

    private void FixedUpdate()
    {
        float TargetXMove = _target.position.x - _homePosition;
        float XPosition = InitXPosition - TargetXMove * XVelocity;
        if (XPosition < MinXLocate) XPosition = MinXLocate;
        else if (XPosition > MaxXLocate) XPosition = MaxXLocate;
        _homeVector.x = XPosition;
        transform.localPosition = _homeVector;
    }
}
