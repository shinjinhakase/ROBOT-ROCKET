using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

// NextButtonにアタッチ
public class LoadStage : MonoBehaviour
{
    [SerializeField] private string scenePrefix;
    [SerializeField] private StageDataBase stageDataBase;

    private int stageNum = -1;
    private Stage stage = new Stage();

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.interactable = false;
    }

    public void SetStageNum(Stage stage)
    {
        this.stageNum = stage.StageNum;
        this.stage = stage;

        button.interactable = true;
    }

    public void OnLoadStage()
    {
        Debug.Log($"Push loadStage : stageNum -> {stageNum}");
        Debug.Log("OnLoadStage : Debug用処理");

        string sceneName = scenePrefix + stageNum;
        StageSelectGlobal.Instance.Stage = stage;
        StageSelectGlobal.Instance.StageDataBase = stageDataBase;
        sceneName = "Play";
        SceneManager.LoadScene(sceneName);
    }
}
