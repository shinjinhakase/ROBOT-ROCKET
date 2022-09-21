using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CollectNumText : MonoBehaviour
{
    private Text _text;

    private CollectionDrop.E_CollectionID id;
    [Tooltip("表示テキストのフォーマット。{0}が個数に変換される。{0:00}のように0埋めを指定することも可能。")]
    [SerializeField] private string _textFormat = "×{0:00}";

    void Awake()
    {
        _text = GetComponent<Text>();
        TextUpdate();
    }

    // テキスト内容を更新する
    public void TextUpdate()
    {
        Collector _collector = Collector.Instance;
        int num = _collector.GetCollectionDatas.GetData(id).num;
        _text.text = string.Format(_textFormat, num);
    }
}
