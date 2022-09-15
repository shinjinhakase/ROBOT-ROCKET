using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

// NextButtonにアタッチ
public class LoadStage : MonoBehaviour
{
    [SerializeField] private string scenePrefix;
    [SerializeField] private StageDataBase stageDataBase;

    private int stageNum = -1;
    private Stage stage = new Stage();

    public void SetStageNum(Stage stage)
    {
        this.stageNum = stage.StageNum;
        this.stage = stage;
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
