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
        // �X�e�[�W�I���{�^�����쐬 �X�e�[�W�ԍ��i�[
        int stageNum = 0;
        foreach(Stage stage in stageDataBase.stageList)
        {
            // �X�e�[�W�I���{�^�����v���n�u����쐬
            GameObject stageButtonObj = Instantiate(stageButtonBase);

            // �R���|�[�l���g�͑��݂��邩�B������Βǉ�
            StageButton stageButton = stageButtonObj.GetComponent<StageButton>();
            if (stageButton == null) stageButton = stageButtonObj.AddComponent<StageButton>();

            // �X�N���[���r���[�ɒǉ�
            stageButton.transform.SetParent(scrollViewContent.transform);

            // �{�^��������
            stageButton.Init(stage, this);

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
        /* �T���l�Ȃǂ�UI���� */
    }
}
