using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measure : UIOpener
{
    [SerializeField] private Transform needle;

    [SerializeField] private IconBoxBuilder iconBuilder;

    [Header("針設定")]
    [SerializeField] private float _maxWeight = 5f;
    [SerializeField] private float _maxAngle = 340f;

    private void Awake()
    {
        if (_maxWeight <= 0)
        {
            _maxWeight = 1;
            throw new Exception("秤の針の最大重量は0以下に設定できません。");
        }
        UpdateRender();
    }

    // 描画を更新するメソッド
    public void UpdateRender()
    {
        iconBuilder?.UpdateBoxes();
        NeedleUpdate();
    }

    // 針の角度を更新する
    public void NeedleUpdate()
    {
        if (needle == null) return;
        float weight = PlayPartsManager.Instance.GetAllWeight() + ForceMove.RobotWeight;
        float angle = weight * _maxAngle / _maxWeight;
        if (angle > _maxAngle) angle = _maxWeight;
        needle.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
