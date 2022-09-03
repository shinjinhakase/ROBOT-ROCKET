using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    private Stage stage;
    private StageSelectManager stageSelectManager;

    public void Init
    (
        Stage stage, 
        StageSelectManager stageSelectManager
    ){
        this.stage = stage;
        this.stageSelectManager = stageSelectManager;
    }

    public void OnSelect()
    {
        stageSelectManager.SelectStage(stage);
    }
}
