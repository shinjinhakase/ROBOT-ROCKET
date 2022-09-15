using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �A�C�R���̏����ꂽ�����Ǘ�����Component
public class IconBoxBuilder : MonoBehaviour
{
    [SerializeField] private IconBox _iconBoxPrefab;
    [SerializeField] private Direction boxDirection = Direction.Up;
    private Vector2 DirectionVector => boxDirection == Direction.Up ? Vector2.up : 
        (boxDirection == Direction.Down ? Vector2.down : 
        (boxDirection == Direction.Right ? Vector2.right : Vector2.left));

    private List<IconBox> iconBoxes = new List<IconBox>();

    // �p�[�c���\���擾���郁�\�b�h
    protected virtual PartsPerformance GetPerformance(PartsInfo.PartsData data) => PlayPartsManager.Instance.GetPerformance(data.id);

    protected void Awake()
    {
        UpdateBoxes();
    }

    // �V���������\�z����
    private void BuildBox(PartsInfo.PartsData data)
    {
        var iconBox = Instantiate(_iconBoxPrefab, transform);
        iconBox.transform.localPosition = DirectionVector * iconBoxes.Count;
        iconBox.SetSprite(GetPerformance(data));
        iconBoxes.Add(iconBox);
    }

    // ���̕`����X�V����
    public void UpdateBoxes()
    {
        PartsInfo _partsInfo = PartsInfo.Instance;
        int length = _partsInfo.Length;
        var dataList = _partsInfo.GetPartsList();
        // �����̔��̕`����X�V����
        for(int i = 0; i < iconBoxes.Count; i++)
        {
            if (i >= length) Destroy(iconBoxes[i].gameObject);
            else
            {
                iconBoxes[i].SetSprite(GetPerformance(dataList[i]));
            }
        }
        // �폜�����I�u�W�F�N�g�����X�g����폜����
        iconBoxes.RemoveAll(data => data == null);
        // �V�������𐶐�����
        if(iconBoxes.Count < length)
        {
            for (int i = iconBoxes.Count; i < length; i++)
            {
                BuildBox(dataList[i]);
            }
        }
    }

    // ������ׂ���������߂�񋓌^
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }
}
