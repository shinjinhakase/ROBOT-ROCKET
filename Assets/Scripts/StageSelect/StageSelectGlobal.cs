using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectGlobal : SingletonDontDestroy<StageSelectGlobal>
{
    private int stageNum = -1;

    public int StageNum
    {
        get { return stageNum; }
        set { stageNum = value; }
    }
}
