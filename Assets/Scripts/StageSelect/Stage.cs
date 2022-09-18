using UnityEngine;
using System;

[Serializable]
public class Stage
{
    public Sprite thumbnail;
    public float goalDistance;
    public string memo;

    private int stageNum = -1;
    private StageProgressData progressData = new StageProgressData();

    public StageProgressData ProgressData
    {
        get { return progressData; }
        set { progressData = value; }
    }
    
    public int StageNum
    {
        get { return stageNum; }
        set 
        {
            stageNum = value;
            ProgressData.StageNum = value;
        }
    }
}
