using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ԍ��̃X�v���C�g���Ǘ�����Component
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

    // �ԍ���ݒ肷��
    public void SetNumber(int Num)
    {
        _num = Num;
        _spriteRenderer.sprite = GetSprite(Num);
    }

    // �X�v���C�g��Null�ɂ���
    public void SetNullSprite()
    {
        _num = -1;
        _spriteRenderer.sprite = null;
    }
}
