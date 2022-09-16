using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConfig : SavableSingletonBase<AudioConfig>
{
    [SerializeField] private float bgmSliderValue = 50f;
    [SerializeField] private float seSliderValue = 50f;

    [SerializeField] private bool isSaved = false;

    public float BgmSliderValue
    {
        get { return bgmSliderValue; }
        set { bgmSliderValue = value; }
    }

    public float SeSliderValue
    {
        get { return seSliderValue; }
        set { seSliderValue = value; }
    }

    public bool IsSaved
    {
        get { return isSaved; }
    }

    public void InitSave(float bgmSliderValue, float seSliderValue)
    {
        this.bgmSliderValue = bgmSliderValue;
        this.seSliderValue = seSliderValue;
        isSaved = true;

        Save();
    }
}
