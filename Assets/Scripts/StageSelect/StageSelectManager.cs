using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] private StageSelectUIManager uIManager;

    [SerializeField] private StageDataBase stageDataBase;
    [SerializeField] private GameObject scrollViewContent;
    [SerializeField] private GameObject numberSelectButtonBase;
    [SerializeField] private LoadStage loadStage;

    private void Start()
    {
        List<Button> buttonList = new List<Button>();

        // ステージ選択ボタンを作成・初期化
        for(int stageNum = 0; stageNum < stageDataBase.stageList.Count; stageNum++)
        {
            Stage stage = stageDataBase.stageList[stageNum];

            // ステージ選択ボタンをプレハブから作成
            GameObject numberSelectButtonObj = Instantiate(numberSelectButtonBase);

            // コンポーネント取得
            NumberSelectButton numberSelectButton
                = numberSelectButtonObj.GetComponent<NumberSelectButton>();
            Button button
                = numberSelectButtonObj.GetComponent<Button>();
            buttonList.Add(button);

            // スクロールビューに追加
            numberSelectButton.transform.SetParent(scrollViewContent.transform);

            // ステージ番号格納
            stage.StageNum = stageNum;

            // ボタン初期化
            numberSelectButton.Init(stage, this);
        }

        // ステージの進捗状況確認
        // クリア済みのステージ番号を確認
        int clearStageNum = CheckClearStage();

        for(int stageNum = 0; stageNum < buttonList.Count; stageNum++)
        {
            Button button = buttonList[stageNum];

            // クリアしているステージ + 1は開けておく
            if(stageNum <= clearStageNum + 1 ) button.interactable = true;
            else                               button.interactable = false;
        }

        // セーブとロードの動作確認
        ProgressData progressData1 = ProgressData.Instance;
        progressData1.SetStageList(stageDataBase.stageList, clearStageNum);
        progressData1.Save();

        progressData1.Reset();

        ProgressData progressData2 = ProgressData.Instance;
        Debug.Log(progressData2.StageProgressDataList);

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

    private int CheckClearStage()
    {
        int clearStageNum = -1;
        foreach (Stage stage in stageDataBase.stageList)
        {
            // クリアしているステージ番号格納
            if (stage.ProgressData.IsClear) clearStageNum = stage.StageNum;
        }

        return clearStageNum;
    }
}