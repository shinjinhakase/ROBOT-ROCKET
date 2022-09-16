using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �A�C�R�����܂񂾔����Ǘ�����Component
public class IconBox : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _iconSpriteRenderer;
    [SerializeField] private IconDirectionForce _iconDirectionForce;

    // �X�v���C�g��ݒ肷��
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
