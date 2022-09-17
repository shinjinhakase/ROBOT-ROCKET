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
        // 進捗データをロード
        ProgressData progressData = ProgressData.Instance;
        CreateStageData(progressData);
        CreateNumButton(progressData);
    }

    private void CreateStageData(ProgressData progressData)
    {
        var spDataList = progressData.StageProgressDataList;

        for (int stageNum = 0; stageNum < stageDataBase.stageList.Count; stageNum++)
        {
            // ステージ情報作成
            Stage stage = stageDataBase.stageList[stageNum];
            stage.StageNum = stageNum;

            // セーブデータが存在する場合はDBに登録し、無い場合は埋め合わせる
            if (stageNum < spDataList.Count) stage.ProgressData = spDataList[stageNum];
            else stage.ProgressData = new StageProgressData();
        }
    }

    private void CreateNumButton(ProgressData progressData)
    {
        for (int stageNum = 0; stageNum < stageDataBase.stageList.Count; stageNum++)
        {
            Stage stage = stageDataBase.stageList[stageNum];

            // ステージ選択ボタンをプレハブから作成
            GameObject numberSelectButtonObj = Instantiate(numberSelectButtonBase, scrollViewContent.transform);

            // ボタン初期化
            NumberSelectButton numberSelectButton
                = numberSelectButtonObj.GetComponent<NumberSelectButton>();
            numberSelectButton.Init(stage, this);

            // ステージ解放処理
            if (stageNum <= progressData.ClearStageNum + 1)
                numberSelectButton.ButtonInteract(true);
            else
                numberSelectButton.ButtonInteract(false);
        }
    }

    public void SelectStage(Stage stage)
    {
        Debug.Log($"Push stageButton : stageNum -> {stage.StageNum}");
        loadStage.SetStageNum(stage);
        uIManager.SelectStage(stage);
    }
}