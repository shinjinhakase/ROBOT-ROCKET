using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 音を鳴らすComponent。オブジェクトを破棄すると音が停止するかもしれない。
public class SoundBeep : MonoBehaviour
{
    [SerializeField] private AudioSource sound;

    // 音を鳴らすメソッド
    public void Beep()
    {
        sound.Play();
    }
}
