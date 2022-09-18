using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageSelectGlobal : SingletonDontDestroy<StageSelectGlobal>
{
    private Stage stage = new Stage();
    private StageDataBase stageDataBase = null;

    public Stage Stage
    {
        get { return stage; }
        set { stage = value; }
    }

    public StageDataBase StageDataBase
    {
        get { return stageDataBase; }
        set { stageDataBase = value; }
    }
}
