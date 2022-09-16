using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUIManager : MonoBehaviour
{
    [SerializeField] private Image thumbnailImage;

    [SerializeField] private Text goalText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timeText;

    [SerializeField] private Text numImageText;
    [SerializeField] private Image noActiveImage;

    private bool isDisplayNoActiveImage = true;

    public void SelectStage(Stage stage)
    {
        thumbnailImage.sprite = stage.thumbnail;

        goalText.text = "Goal\n"
            + stage.goalDistance.ToString("f2") + "m";
        scoreText.text = "Best Flight\n"
            + stage.ProgressData.BestDistance.ToString("f2") + "m";
        timeText.text = "Best Time\n"
            + stage.ProgressData.BestTime.ToString("f2") + "sec";

        numImageText.text = (stage.StageNum + 1).ToString("00");
        isDisplayNoActiveImage = false;
        noActiveImage.gameObject.SetActive(isDisplayNoActiveImage);
    }
}
