using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measure : UIOpener
{
    [SerializeField] private Transform needle;

    [SerializeField] private Shaker measureSprite;
    [SerializeField] private IconBoxBuilder iconBuilder;

    [Header("�j�ݒ�")]
    [SerializeField] private float _warningWeight = 2f;
    [SerializeField] private float _maxWeight = 5f;
    [SerializeField] private float _maxAngle = 340f;

    private void Awake()
    {
        if (_maxWeight <= 0)
        {
            _maxWeight = 1;
            throw new Exception("���̐j�̍ő�d�ʂ�0�ȉ��ɐݒ�ł��܂���B");
        }
        UpdateRender();
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
        UpdateRender();
    }

    // �`����X�V���郁�\�b�h
    public void UpdateRender()
    {
        iconBuilder?.UpdateBoxes();
        NeedleUpdate();
    }

    // �j�̊p�x���X�V����
    public void NeedleUpdate()
    {
        // �j�̊p�x����
        if (needle == null) return;
        float weight = PlayPartsManager.Instance.GetAllWeight() + ForceMove.RobotWeight;
        float angle = weight * _maxAngle / _maxWeight;
        if (angle > _maxAngle) angle = _maxWeight;
        needle.rotation = Quaternion.Euler(0, 0, -angle);

        // ���̐U������
        measureSprite.DoShake = weight >= _warningWeight;
    }
}
