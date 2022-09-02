using UnityEngine;
using System;

[Serializable]
public class Stage
{
    public Sprite thumbnail;
    public float goalDistance;
    public string memo;

    private int stageNum;
    private float bestDistance;
    private float bestTime;

    public int StageNum
    {
        get { return stageNum; }
        set { stageNum = value; }
    }

    public float BestDicetance
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
