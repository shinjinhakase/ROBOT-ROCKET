using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSelectButton : MonoBehaviour
{
    [SerializeField] private Text numText;

    private Stage stage;
    private StageSelectManager stageSelectManager;

    public void Init
    (
        Stage stage, 
        StageSelectManager stageSelectManager
    ){
        this.stage = stage;
        this.stageSelectManager = stageSelectManager;

        numText.text = (stage.StageNum + 1).ToString("00");
    }

    public void ButtonInteract(bool isInteractable)
    {
        Button button = GetComponent<Button>();
        button.interactable = isInteractable;
    }

    public void OnSelect()
    {
        stageSelectManager.SelectStage(stage);
    }
}
