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

        if (!audioConfig.IsSaved) // �Z�[�u�f�[�^�����Ȃ�쐬
        {
            audioConfig.InitSave(firstBgmValumeSlider, firstSeValumeSlider);
        }
        else // �Z�[�u�f�[�^�L��Ȃ烍�[�h
        {
            firstBgmValumeSlider = audioConfig.BgmSliderValue;
            firstSeValumeSlider = audioConfig.SeSliderValue;
        }

        // �����l���X���C�_�[�ƃ~�L�T�[�ɔ��f
        bgmSlider.value = firstBgmValumeSlider;
        seSlider.value = firstSeValumeSlider;
        audioValume.SetAudioMixerBGM(firstBgmValumeSlider);
        audioValume.SetAudioMixerBGM(firstSeValumeSlider);
    }
}
