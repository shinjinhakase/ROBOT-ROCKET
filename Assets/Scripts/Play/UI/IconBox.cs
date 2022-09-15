using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイコンを含んだ箱を管理するComponent
public class IconBox : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _iconSpriteRenderer;

    // スプライトを設定する
    public void SetSprite(Sprite iconSprite)
    {
        _iconSpriteRenderer.sprite = iconSprite;
    }
    public void SetSprite(PartsPerformance performance)
    {
        _iconSpriteRenderer.sprite = performance.iconSprite;
    }
}
