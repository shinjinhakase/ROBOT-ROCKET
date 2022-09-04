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
        // �X�e�[�W�I���{�^�����쐬 �X�e�[�W�ԍ��i�[
        int stageNum = 0;
        foreach (Stage stage in stageDataBase.stageList)
        {
            // �X�e�[�W�I���{�^�����v���n�u����쐬
            GameObject numberSelectButtonObj = Instantiate(numberSelectButtonBase);

            // �R���|�[�l���g�͑��݂��邩�B������Βǉ�
            NumberSelectButton numberSelectButton
                = numberSelectButtonObj.GetComponent<NumberSelectButton>();
            if (numberSelectButton == null)
                numberSelectButton = numberSelectButtonObj.AddComponent<NumberSelectButton>();

            // �X�N���[���r���[�ɒǉ�
            numberSelectButton.transform.SetParent(scrollViewContent.transform);

            // �{�^��������
            numberSelectButton.Init(stage, this);

            // �X�e�[�W�ԍ��i�[
            stage.StageNum = stageNum;

            stageNum++;
        }

        // �Z�[�u�ƃ��[�h�̓���m�F
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