using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] private StageSelectUIManager uIManager;

    [SerializeField] private StageDataBase stageDataBase;
    [SerializeField] private GameObject scrollViewContent;
    [SerializeField] private GameObject numberSelectButtonBase;
    [SerializeField] private LoadStage loadStage;

    private void Start()
    {
        // ステージ選択ボタンを作成 ステージ番号格納
        int stageNum = 0;
        foreach (Stage stage in stageDataBase.stageList)
        {
            // ステージ選択ボタンをプレハブから作成
            GameObject numberSelectButtonObj = Instantiate(numberSelectButtonBase);

            // コンポーネントは存在するか。無ければ追加
            NumberSelectButton numberSelectButton
                = numberSelectButtonObj.GetComponent<NumberSelectButton>();
            if (numberSelectButton == null)
                numberSelectButton = numberSelectButtonObj.AddComponent<NumberSelectButton>();

            // スクロールビューに追加
            numberSelectButton.transform.SetParent(scrollViewContent.transform);

            // ボタン初期化
            numberSelectButton.Init(stage, this);

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
        uIManager.SelectStage(stage);
    }
}