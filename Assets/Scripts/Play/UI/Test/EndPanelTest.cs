using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPanelTest : MonoBehaviour
{
    public GameEndPanel gameEndPanel;
    public bool isGameSuccess;

    void Start()
    {
        if (isGameSuccess) gameEndPanel.ClearInit();
        else gameEndPanel.GameOverInit();
    }
}
