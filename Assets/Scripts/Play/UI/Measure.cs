using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measure : UIOpener
{
    [SerializeField] private Transform needle;

    [SerializeField] private Shaker measureSprite;
    [SerializeField] private IconBoxBuilder iconBuilder;

    [Header("êjê›íË")]
    [SerializeField] private float _warningWeight = 2f;
    [SerializeField] private float _maxWeight = 5f;
    [SerializeField] private float _maxAngle = 340f;

    private void Awake()
    {
        if (_maxWeight <= 0)
        {
            _maxWeight = 1;
            throw new Exception("îâÇÃêjÇÃç≈ëÂèdó ÇÕ0à»â∫Ç…ê›íËÇ≈Ç´Ç‹ÇπÇÒÅB");
        }
        UpdateRender();
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
        UpdateRender();
    }

    // ï`âÊÇçXêVÇ∑ÇÈÉÅÉ\ÉbÉh
    public void UpdateRender()
    {
        iconBuilder?.UpdateBoxes();
        NeedleUpdate();
    }

    // êjÇÃäpìxÇçXêVÇ∑ÇÈ
    public void NeedleUpdate()
    {
        // êjÇÃäpìxí≤êÆ
        if (needle == null) return;
        float weight = PlayPartsManager.Instance.GetAllWeight() + ForceMove.RobotWeight;
        float angle = weight * _maxAngle / _maxWeight;
        if (angle > _maxAngle) angle = _maxWeight;
        needle.rotation = Quaternion.Euler(0, 0, -angle);

        // îâÇÃêUìÆêßå‰
        measureSprite.DoShake = weight >= _warningWeight;
    }
}
