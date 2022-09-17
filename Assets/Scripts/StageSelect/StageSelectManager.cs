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
        // �i���f�[�^�����[�h
        ProgressData progressData = ProgressData.Instance;
        CreateStageData(progressData);
        CreateNumButton(progressData);
    }

    private void CreateStageData(ProgressData progressData)
    {
        var spDataList = progressData.StageProgressDataList;

        for (int stageNum = 0; stageNum < stageDataBase.stageList.Count; stageNum++)
        {
            // �X�e�[�W���쐬
            Stage stage = stageDataBase.stageList[stageNum];
            stage.StageNum = stageNum;

            // �Z�[�u�f�[�^�����݂���ꍇ��DB�ɓo�^���A�����ꍇ�͖��ߍ��킹��
            if (stageNum < spDataList.Count) stage.ProgressData = spDataList[stageNum];
            else stage.ProgressData = new StageProgressData();
        }
    }

    private void CreateNumButton(ProgressData progressData)
    {
        for (int stageNum = 0; stageNum < stageDataBase.stageList.Count; stageNum++)
        {
            Stage stage = stageDataBase.stageList[stageNum];

            // �X�e�[�W�I���{�^�����v���n�u����쐬
            GameObject numberSelectButtonObj = Instantiate(numberSelectButtonBase, scrollViewContent.transform);

            // �{�^��������
            NumberSelectButton numberSelectButton
                = numberSelectButtonObj.GetComponent<NumberSelectButton>();
            numberSelectButton.Init(stage, this);

            // �X�e�[�W�������
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