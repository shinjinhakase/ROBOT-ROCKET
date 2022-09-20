using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���W�v�f�̃h���b�v�A�C�e������������Component
public class CollectionDrop : PickableGimick
{
    [SerializeField] private E_CollectionID id; // �l������A�C�e��ID
    [SerializeField] private int num = 1;       // �l�����鐔

    public override void OnSceneStart()
    {
        Collector.Instance.RegisterItem(id, num);
    }

    protected override void PickedActionOnlyPlay(MainRobot mainRobot)
    {
        Collector.Instance.GetItem(id, num);
    }

    // ���W�v�f�A�C�e����ID
    public enum E_CollectionID
    {
        Coin
    }
}
