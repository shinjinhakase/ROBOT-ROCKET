using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

// NextButton�ɃA�^�b�`
public class LoadStage : MonoBehaviour
{
    [SerializeField] private string scenePrefix;

    private int stageNum = -1;

    public void SetStageNum(int stageNum)
    {
        this.stageNum = stageNum;
    }

    public void OnLoadStage()
    {
        Debug.Log($"Push loadStage : stageNum -> {stageNum}");
        Debug.Log("OnLoadStage : Debug�p����");
        string sceneName = scenePrefix + stageNum;
        sceneName = "Play";
        SceneManager.LoadScene(sceneName);
    }
}
