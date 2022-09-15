using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイコンの書かれた箱を管理するComponent
public class IconBoxBuilder : MonoBehaviour
{
    [SerializeField] private IconBox _iconBoxPrefab;
    [SerializeField] private Direction boxDirection = Direction.Up;
    private Vector2 DirectionVector => boxDirection == Direction.Up ? Vector2.up : 
        (boxDirection == Direction.Down ? Vector2.down : 
        (boxDirection == Direction.Right ? Vector2.right : Vector2.left));

    private List<IconBox> iconBoxes = new List<IconBox>();

    // パーツ性能を取得するメソッド
    protected virtual PartsPerformance GetPerformance(PartsInfo.PartsData data) => PlayPartsManager.Instance.GetPerformance(data.id);

    protected void Awake()
    {
        UpdateBoxes();
    }

    // 新しく箱を構築する
    private void BuildBox(PartsInfo.PartsData data)
    {
        var iconBox = Instantiate(_iconBoxPrefab, transform);
        iconBox.transform.localPosition = DirectionVector * iconBoxes.Count;
        iconBox.SetSprite(GetPerformance(data));
        iconBoxes.Add(iconBox);
    }

    // 箱の描画を更新する
    public void UpdateBoxes()
    {
        PartsInfo _partsInfo = PartsInfo.Instance;
        int length = _partsInfo.Length;
        var dataList = _partsInfo.GetPartsList();
        // 既存の箱の描画を更新する
        for(int i = 0; i < iconBoxes.Count; i++)
        {
            if (i >= length) Destroy(iconBoxes[i].gameObject);
            else
            {
                iconBoxes[i].SetSprite(GetPerformance(dataList[i]));
            }
        }
        // 削除したオブジェクトをリストから削除する
        iconBoxes.RemoveAll(data => data == null);
        // 新しく箱を生成する
        if(iconBoxes.Count < length)
        {
            for (int i = iconBoxes.Count; i < length; i++)
            {
                BuildBox(dataList[i]);
            }
        }
    }

    // 箱を並べる方向を決める列挙型
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }
}
