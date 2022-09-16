using UnityEngine;
using UnityEngine.SceneManagement;
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

        string sceneName = scenePrefix + stageNum;
        StageSelectGlobal.Instance.Stage = stage;
        StageSelectGlobal.Instance.StageDataBase = stageDataBase;
        SceneManager.LoadScene(sceneName);
    }
}
