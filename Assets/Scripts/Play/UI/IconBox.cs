using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �A�C�R�����܂񂾔����Ǘ�����Component
public class IconBox : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _iconSpriteRenderer;

    // �X�v���C�g��ݒ肷��
    public void SetSprite(Sprite iconSprite)
    {
        _iconSpriteRenderer.sprite = iconSprite;
    }
    public void SetSprite(PartsPerformance performance)
    {
        _iconSpriteRenderer.sprite = performance.iconSprite;
    }
}
