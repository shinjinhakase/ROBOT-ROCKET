using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 保留中
public class ValumeManager : SingletonDontDestroy<ValumeManager>
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    [Header("初期値")]
    [SerializeField] private float bgmValume = 0.5f;
    [SerializeField] private float seValume = 0.5f;

    protected override void Awake()
    {
        bgmSlider.value = bgmValume;
        seSlider.value = seValume;
        base.Awake();
    }

    public float BgmValume
    {
        get { return bgmValume; }
        private set { bgmValume = value; }
    }

    public float SeValume
    {
        get { return seValume; }
        private set { seValume = value; }
    }

    // BGM・SEの音量をスライダーで変化させる
    public void OnBgmValumeChange(AudioSource ac)
    {
        BgmValume = bgmSlider.value;
        ac.volume = BgmValume;
    }
    public void OnSeValumeChange(AudioSource ac)
    {
        SeValume = seSlider.value;
        ac.volume = SeValume;
    }

    // BGM・SEの音量設定をAudioSourceに反映させる
    public void InitBgmValume(AudioSource ac)
    {
        ac.volume = BgmValume;
    }
    public void InitSeValume(AudioSource ac)
    {
        ac.volume = SeValume;
    }
}
