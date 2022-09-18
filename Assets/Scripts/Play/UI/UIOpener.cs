using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UIウィンドウを開いたり閉じたりするComponent。
public class UIOpener : MonoBehaviour
{
    // パネルを開く（有効化する）
    public virtual void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    // パネルを閉じる（無効化する）
    public virtual void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
