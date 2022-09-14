using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    [SerializeField] private Transform _target; // 追従対象（基本カメラ）

    [Header("移動設定")]
    [Tooltip("基準地点（基本はスタート地点になるかな？）")]
    [SerializeField] private float _homePosition = 0;
    [Tooltip("基準地点でのX座標の位置")]
    [SerializeField] private float InitXPosition;
    [Tooltip("左方向に動ける限界距離")]
    [SerializeField] private float MinXLocate;
    [Tooltip("右方向に動ける限界距離")]
    [SerializeField] private float MaxXLocate;

    [Tooltip("横方向の移動速度（1で追従対象の移動と等速）")]
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
