using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 収集要素のドロップアイテムを処理するComponent
public class CollectionDrop : PickableGimick
{
    [SerializeField] private E_CollectionID id; // 獲得するアイテムID
    [SerializeField] private int num = 1;       // 獲得する数

    public override void OnSceneStart()
    {
        Collector.Instance.RegisterItem(id, num);
    }

    protected override void PickedActionOnlyPlay(MainRobot mainRobot)
    {
        Collector.Instance.GetItem(id, num);
    }

    // 収集要素アイテムのID
    public enum E_CollectionID
    {
        Coin
    }
}
