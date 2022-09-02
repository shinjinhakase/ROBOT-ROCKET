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
        StageSelectManager stageSelectManager, 
        int stageNum
    ){
        this.stage = stage;
        this.stageSelectManager = stageSelectManager;
        stage.StageNum = stageNum;
    }

    public void OnSelect()
    {
        stageSelectManager.SelectStage(stage);
    }
}
