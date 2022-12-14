using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measure : UIOpener
{
    [SerializeField] private Transform needle;

    [SerializeField] private Shaker measureSprite;
    [SerializeField] private IconBoxBuilder iconBuilder;

    [Header("針設定")]
    [SerializeField] private float _warningWeight = 2f;
    [SerializeField] private float _maxWeight = 5f;
    [SerializeField] private float _maxAngle = 340f;
    [SerializeField] private bool _turnRight = false;

    private void Awake()
    {
        if (_maxWeight <= 0)
        {
            _maxWeight = 1;
            throw new Exception("秤の針の最大重量は0以下に設定できません。");
        }
        UpdateRender();
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
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
        // 針の角度調整
        if (needle == null) return;
        float weight = PlayPartsManager.Instance.GetAllWeight() + ForceMove.RobotWeight;
        float angle = weight * _maxAngle / _maxWeight;
        if (angle > _maxAngle) angle = _maxWeight;
        if (_turnRight) needle.rotation = Quaternion.Euler(0, 0, -angle);
        else needle.rotation = Quaternion.Euler(0, 0, angle);

        // 秤の振動制御
        measureSprite.DoShake = weight >= _warningWeight;
    }
}
