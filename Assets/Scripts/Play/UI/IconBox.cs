using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイコンを含んだ箱を管理するComponent
public class IconBox : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _iconSpriteRenderer;
    [SerializeField] private IconDirectionForce _iconDirectionForce;

    // スプライトを設定する
    public void SetSprite(Sprite iconSprite)
    {
        _iconSpriteRenderer.sprite = iconSprite;
    }
    public void SetSprite(PartsPerformance performance)
    {
        _iconSpriteRenderer.sprite = performance.iconSprite;
    }
    public void SetSprite(PartsPerformance performance, PartsInfo.PartsData data)
    {
        SetSprite(performance);
        if (_iconDirectionForce != null) _iconDirectionForce.SetRotate(data);
    }
}
