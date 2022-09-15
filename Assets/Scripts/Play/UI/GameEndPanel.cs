using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �Q�[���I�����ɕ\�������E�B���h�EUI���Ǘ�����Component
public class GameEndPanel : UIOpener
{
    [SerializeField] private Text resultText;
    [SerializeField] private Text scoreText;

    // �Q�[���N���A���̏��������\�b�h
    public void ClearInit()
    {
        InitPanel("�X�e�[�W�N���A�I");
    }
    // �Q�[���I�[�o�[���̏��������\�b�h
    public void GameOverInit()
    {
        InitPanel("�Q�[���I�[�o�[�c");
    }

    // �p�l���\�����̏��������\�b�h
    public void InitPanel(string resultMessage)
    {
        PlaySceneController _playSceneController = PlaySceneController.Instance;
        resultText.text = resultMessage;

        string highscoreStr;
        if (_playSceneController.IsLoadStage)
            highscoreStr = _playSceneController.CurrentStage.ProgressData.BestDistance.ToString();
        else
        {
            Debug.Log("isLoadStage = false : �n�C�X�R�A�͓ǂݍ��܂�܂���");
            highscoreStr = "???";
        }

        scoreText.text = "Score�F" + _playSceneController.Score
                + "\nHigh Score�F" + highscoreStr;

        base.OpenPanel();
    }
}
