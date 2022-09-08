using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �Q�[���I�����ɕ\�������E�B���h�EUI���Ǘ�����Component
public class GameEndPanel : MonoBehaviour
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
        scoreText.text = "Score�F" + _playSceneController.Score + "\nHigh Score�F???";
        gameObject.SetActive(true);
    }
    // �p�l�������
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
