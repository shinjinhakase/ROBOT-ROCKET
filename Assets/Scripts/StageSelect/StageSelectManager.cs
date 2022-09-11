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

        // �X�e�[�W�I���{�^�����쐬�E������
        for(int stageNum = 0; stageNum < stageDataBase.stageList.Count; stageNum++)
        {
            Stage stage = stageDataBase.stageList[stageNum];

            // �X�e�[�W�I���{�^�����v���n�u����쐬
            GameObject numberSelectButtonObj = Instantiate(numberSelectButtonBase);

            // �R���|�[�l���g�擾
            NumberSelectButton numberSelectButton
                = numberSelectButtonObj.GetComponent<NumberSelectButton>();
            Button button
                = numberSelectButtonObj.GetComponent<Button>();
            buttonList.Add(button);

            // �X�N���[���r���[�ɒǉ�
            numberSelectButton.transform.SetParent(scrollViewContent.transform);

            // �X�e�[�W�ԍ��i�[
            stage.StageNum = stageNum;

            // �{�^��������
            numberSelectButton.Init(stage, this);
        }

        // �X�e�[�W�̐i���󋵊m�F
        // �N���A�ς݂̃X�e�[�W�ԍ����m�F
        int clearStageNum = CheckClearStage();

        for(int stageNum = 0; stageNum < buttonList.Count; stageNum++)
        {
            Button button = buttonList[stageNum];

            // �N���A���Ă���X�e�[�W + 1�͊J���Ă���
            if(stageNum <= clearStageNum + 1 ) button.interactable = true;
            else                               button.interactable = false;
        }

        // �Z�[�u�ƃ��[�h�̓���m�F
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
            // �N���A���Ă���X�e�[�W�ԍ��i�[
            if (stage.ProgressData.IsClear) clearStageNum = stage.StageNum;
        }

        return clearStageNum;
    }
}