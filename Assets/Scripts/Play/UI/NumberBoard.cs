using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 番号のスプライトを管理するComponent
[RequireComponent(typeof(SpriteRenderer))]
public class NumberBoard : MonoBehaviour
{
    [SerializeField] private int _num = -1;
    [SerializeField] private List<Sprite> _numberSprites = new List<Sprite>();

    private Sprite GetSprite(int Num) => Num >= 0 && Num < _numberSprites.Count ? _numberSprites[Num] : null;

    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetNumber(_num);
    }

    // 番号を設定する
    public void SetNumber(int Num)
    {
        _num = Num;
        _spriteRenderer.sprite = GetSprite(Num);
    }

    // スプライトをNullにする
    public void SetNullSprite()
    {
        _num = -1;
        _spriteRenderer.sprite = null;
    }
}
