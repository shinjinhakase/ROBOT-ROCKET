using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] private StageDataBase stageDataBase;
    [SerializeField] private GameObject scrollViewContent;
    [SerializeField] private GameObject stageButtonBase;
    [SerializeField] private LoadStage loadStage;

    private void Start()
    {
        // ステージ選択ボタンを作成 ステージ番号格納
        int stageNum = 0;
        foreach(Stage stage in stageDataBase.stageList)
        {
            // ステージ選択ボタンをプレハブから作成
            GameObject stageButtonObj = Instantiate(stageButtonBase);

            // コンポーネントは存在するか。無ければ追加
            StageButton stageButton = stageButtonObj.GetComponent<StageButton>();
            if (stageButton == null) stageButton = stageButtonObj.AddComponent<StageButton>();

            // スクロールビューに追加
            stageButton.transform.SetParent(scrollViewContent.transform);

            // ボタン初期化
            stageButton.Init(stage, this);

            // ステージ番号格納
            stage.StageNum = stageNum;

            stageNum++;
        }

        // セーブとロードの動作確認
        /*
        StagesProgressSaveManager saveManager = new StagesProgressSaveManager();
        saveManager.Save(new StagesProgressSaveData(stageDataBase.stageList));
        StagesProgressSaveData saveData = saveManager.Load();
        Debug.Log($"Operation check : saveData -> {saveData}");
        */
    }

    public void SelectStage(Stage stage)
    {
        Debug.Log($"Push stageButton : stageNum -> {stage.StageNum}");
        loadStage.SetStageNum(stage.StageNum);
        /* サムネなどのUI処理 */
    }
}
