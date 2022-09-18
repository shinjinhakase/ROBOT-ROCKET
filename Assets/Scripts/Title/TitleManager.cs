using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private AudioValume audioValume;

    [Header("BGM")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private float firstBgmValumeSlider;
    [Header("SE")]
    [SerializeField] private Slider seSlider;
    [SerializeField] private float firstSeValumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        AudioConfig audioConfig = AudioConfig.Instance;

        if (!audioConfig.IsSaved) // セーブデータ無しなら作成
        {
            audioConfig.InitSave(firstBgmValumeSlider, firstSeValumeSlider);
        }
        else // セーブデータ有りならロード
        {
            firstBgmValumeSlider = audioConfig.BgmSliderValue;
            firstSeValumeSlider = audioConfig.SeSliderValue;
        }

        // 初期値をスライダーとミキサーに反映
        bgmSlider.value = firstBgmValumeSlider;
        seSlider.value = firstSeValumeSlider;
        audioValume.SetAudioMixerBGM(firstBgmValumeSlider);
        audioValume.SetAudioMixerBGM(firstSeValumeSlider);
    }
}
