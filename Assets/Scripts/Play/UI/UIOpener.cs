using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI�E�B���h�E���J����������肷��Component�B
public class UIOpener : MonoBehaviour
{
    // �p�l�����J���i�L��������j
    public virtual void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    // �p�l�������i����������j
    public virtual void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
