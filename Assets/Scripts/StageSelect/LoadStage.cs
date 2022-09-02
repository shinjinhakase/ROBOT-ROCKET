using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// NextButtonにアタッチ
public class LoadStage : MonoBehaviour
{
    [SerializeField] private string scenePrefix;

    private int stageNum = 0;

    public void SetStageNum(int stageNum)
    {
        this.stageNum = stageNum;
    }

    public void OnLoadStage()
    {
        Debug.Log($"Push loadStage : stageNum -> {stageNum}");
        string sceneName = scenePrefix + stageNum;
        SceneManager.LoadScene(sceneName);
    }
}
