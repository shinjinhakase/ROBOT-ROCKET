using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����炷Component�B�I�u�W�F�N�g��j������Ɖ�����~���邩������Ȃ��B
public class SoundBeep : MonoBehaviour
{
    [SerializeField] private AudioSource sound;

    // ����炷���\�b�h
    public void Beep()
    {
        sound.Play();
    }
}
