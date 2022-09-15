using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ゲーム終了時に表示されるウィンドウUIを管理するComponent
public class GameEndPanel : UIOpener
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

        string highscoreStr;
        if (_playSceneController.IsLoadStage)
            highscoreStr = _playSceneController.CurrentStage.ProgressData.BestDistance.ToString();
        else
        {
            Debug.Log("isLoadStage = false : ハイスコアは読み込まれません");
            highscoreStr = "???";
        }

        scoreText.text = "Score：" + _playSceneController.Score
                + "\nHigh Score：" + highscoreStr;

        base.OpenPanel();
    }
}
