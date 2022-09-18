using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StageProgressData
{
    // •â•î•ñ
    [SerializeField] private int stageNum = -1;

    // ƒƒCƒ“î•ñ
    [SerializeField] private bool isClear = false;
    [SerializeField] private float bestDistance = 0f;
    [SerializeField] private float bestTime = 999.99f;

    public int StageNum
    {
        get { return stageNum; }
        set { stageNum = value; }
    }

    public bool IsClear
    {
        get { return isClear; }
        set { isClear = value; }
    }

    public float BestDistance
    {
        get { return bestDistance; }
        set { bestDistance = value; }
    }

    public float BestTime
    {
        get { return bestTime; }
        set { bestTime = value; }
    }
}
