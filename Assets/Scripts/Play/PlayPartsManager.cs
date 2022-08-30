using System;
using UnityEngine;

// プレイシーンで使用するアイテムを管理するComponent。
// 道中でのアイテムの入手・使用、重量計算はこっちを介する。
public class PlayPartsManager : SingletonMonoBehaviourInSceneBase<PlayPartsManager>
{
    private PartsInfo partsInfo;
    [SerializeField] private PartsPerformanceData partsPerformanceData;

    private void Start()
    {
        partsInfo = PartsInfo.Instance;
    }

    // 総重量を計算して返す
    public float GetAllWeight()
    {
        float allWeight = 0f;
        var datas = partsInfo.GetPartsList();
        datas.ForEach(data => allWeight += partsPerformanceData.getData(data.id).m);
        return allWeight;
    }

    // パーツを使う（使うパーツのデータと、生まれる力を返す）
    public void UseParts(out PartsInfo.PartsData data, out IForce force)
    {
        // 使用するパーツのデータを取得する
        if (partsInfo.Length == 0) throw new Exception("使用するパーツがありません。");
        data = partsInfo.GetParts(0);
        var performance = partsPerformanceData.getData(data.id);
        // リストからアイテムを除外する
        partsInfo.RemoveParts();

        // 加える力を構築する
        switch (performance.forceType)
        {
            case PartsPerformance.E_ForceType.Bomb:
                force = new ImpulseForce(data.angle, performance.F, performance.m);
                break;
            case PartsPerformance.E_ForceType.Rocket:
            case PartsPerformance.E_ForceType.Propeller:
                force = new PressForce(data.angle, performance.F, performance.t, performance.k, performance.m);
                break;
            case PartsPerformance.E_ForceType.Glider:
            case PartsPerformance.E_ForceType.CollisionForce:
            default:
                throw new Exception("ロボットに加える力を構築できません。");
        }
    }

    // デバッグ用。使用パーツリストにテストパーツを追加する。
    [ContextMenu("Debug/GetTestParts")]
    private void AddTestParts()
    {
        var data = new PartsInfo.PartsData();
        data.id = PartsPerformance.E_PartsID.TestParts;
        data.angle = 80;
        partsInfo.AddParts(data);
    }
}
