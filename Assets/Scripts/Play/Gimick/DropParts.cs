using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DropParts : GimickBase
{
    [SerializeField] private SpriteRenderer _iconSpriteRenderer;
    [SerializeField] private IconDirectionForce _iconDirectionForce;

    // 取得するパーツの情報
    [SerializeField] private PartsInfo.PartsData partsData;
    private bool Picked = false;

    public override void OnSceneStart()
    {
        if (_iconSpriteRenderer) StartCoroutine(SetIconSprite());
        if (_iconDirectionForce) _iconDirectionForce.SetRotate(partsData);
    }

    // ギミックをリセットするメソッド
    public override void ResetGimick() {
        gameObject.SetActive(true);
        Picked = false;
    }

    // 他の当たり判定と衝突した際のアイテムを拾う処理
    public void PickItem(Collider2D other)
    {
        if (!other.TryGetComponent(out MainRobot robot) || Picked) return;
        Picked = true;
        // ロボットの重量を増やし、使用パーツリストの一番最後に獲得パーツを追加する
        PlayPartsManager.Instance.GetParts(partsData, out PartsPerformance performance);
        ForceMove move = robot._move;
        move.SetWeight(move.GetWeight() + performance.m);

        // 自身を無効化する
        gameObject.SetActive(false);
    }

    // アイコンのスプライトを設定する
    public IEnumerator SetIconSprite()
    {
        while (true)
        {
            PlayPartsManager _instance = PlayPartsManager.Instance;
            if (_instance)
            {
                _iconSpriteRenderer.sprite = _instance.GetPerformance(partsData.id).iconSprite;
                yield break;
            }
            yield return null;
        }
    }
}
