using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DropParts : MonoBehaviour
{
    // 取得するパーツの情報
    [SerializeField] private PartsInfo.PartsData partsData;

    // 他の当たり判定と衝突した際のアイテムを拾う処理
    public void PickItem(Collider2D other)
    {
        if (!other.TryGetComponent(out MainRobot robot)) return;
        // ロボットの重量を増やし、使用パーツリストの一番最後に獲得パーツを追加する
        PlayPartsManager.Instance.GetParts(partsData, out PartsPerformance performance);
        ForceMove move = robot._move;
        move.SetWeight(move.GetWeight() + performance.m);

        // 自身を破棄する
        Destroy(gameObject);
    }
}
