using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioValume : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    private float firstValue = 1.0f;

    void Start()
    {
        bgmSlider.value = firstValue;
        seSlider.value = firstValue;
        SetAudioMixerBGM(firstValue);
        SetAudioMixerSE(firstValue);

        //スライダーを動かした時の処理を登録
        bgmSlider.onValueChanged.AddListener(SetAudioMixerBGM);
        seSlider.onValueChanged.AddListener(SetAudioMixerSE);
    }

    //BGM
    public void SetAudioMixerBGM(float value)
    {
        //5段階補正
        value /= 5;
        //-80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
        //audioMixerに代入
        audioMixer.SetFloat("BGM", volume);
        Debug.Log($"BGM:{volume}");
    }

    //SE
    public void SetAudioMixerSE(float value)
    {
        //5段階補正
        value /= 5;
        //-80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
        //audioMixerに代入
        audioMixer.SetFloat("SE", volume);
        Debug.Log($"SE:{volume}");
    }
}
