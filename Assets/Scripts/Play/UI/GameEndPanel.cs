using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ゲーム終了時に表示されるウィンドウUIを管理するComponent
public class GameEndPanel : MonoBehaviour
{
    [SerializeField] private Text resultText;
    [SerializeField] private Text scoreText;

    // ゲームクリア時の初期化メソッド
    public void ClearInit()
    {
        InitPanel("ステージクリア！");
    }
    // ゲームオーバー時の初期化メソッド
    public void GameOverInit()
    {
        InitPanel("ゲームオーバー…");
    }

    // パネル表示時の初期化メソッド
    public void InitPanel(string resultMessage)
    {
        PlaySceneController _playSceneController = PlaySceneController.Instance;
        resultText.text = resultMessage;
        scoreText.text = "Score：" + _playSceneController.Score + "\nHigh Score：???";
        gameObject.SetActive(true);
    }
    // パネルを閉じる
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
