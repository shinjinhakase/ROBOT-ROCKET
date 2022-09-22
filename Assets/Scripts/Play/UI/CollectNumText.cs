using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CollectNumText : MonoBehaviour
{
    private Text _text;

    private CollectionDrop.E_CollectionID id;
    [Tooltip("�\���e�L�X�g�̃t�H�[�}�b�g�B{0}�����ɕϊ������B{0:00}�̂悤��0���߂��w�肷�邱�Ƃ��\�B")]
    [SerializeField] private string _textFormat = "�~{0:00}";

    void Awake()
    {
        _text = GetComponent<Text>();
        TextUpdate();
    }

    // �e�L�X�g���e���X�V����
    public void TextUpdate()
    {
        Collector _collector = Collector.Instance;
        int num = _collector.GetCollectionDatas.GetData(id).num;
        _text.text = string.Format(_textFormat, num);
    }
}
