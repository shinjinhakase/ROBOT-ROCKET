using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IconBox))]
public class DropParts : PickableGimick
{
    private IconBox _iconBox;

    // 取得するパーツの情報
    [SerializeField] private PartsInfo.PartsData partsData;

    public override void Awake()
    {
        base.Awake();
        _iconBox = GetComponent<IconBox>();
    }

    public override void OnSceneStart()
    {
        StartCoroutine(SetIconSprite());
    }

    protected override void PickedActionOnlyPlay(MainRobot mainRobot)
    {
        // ロボットの重量を増やし、使用パーツリストの一番最後に獲得パーツを追加する
        PlayPartsManager.Instance.GetParts(partsData, out PartsPerformance performance);
        mainRobot._move.AddWeightByPartsPerformance(performance);
    }

    // アイコンのスプライトを設定する
    public IEnumerator SetIconSprite()
    {
        while (true)
        {
            PlayPartsManager _instance = PlayPartsManager.Instance;
            if (_instance)
            {
                _iconBox.SetSprite(_instance.GetPerformance(partsData.id), partsData);
                yield break;
            }
            yield return null;
        }
    }
}
