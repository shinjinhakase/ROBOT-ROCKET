using System;
using UnityEngine;

public class PlayPartsManager : SingletonMonoBehaviourInSceneBase<PlayPartsManager>
{
    [SerializeField] private ForceMove robot;   // 力を加える対象
    [SerializeField] private PartsPerformanceData partsPerformanceData;

    // その時点での総重量を計算して返す
    public float GetAllWeight()
    {
        float allWeight = 0f;
        var datas = PartsInfo.Instance.GetPartsList();
        datas.ForEach(data => allWeight += partsPerformanceData.getData(data.id).m);
        return allWeight;
    }

    [ContextMenu("Debug/UseParts")]
    void UseParts()
    {
        // 使用するパーツのデータを取得する
        // （使用したアイテムの消去は力が消えたら行う）
        PartsInfo PI = PartsInfo.Instance;
        if (PI.Length == 0) throw new Exception("使用するパーツがありません。");
        var data = PI.GetParts(0);
        var performance = partsPerformanceData.getData(data.id);

        // 加える力を構築する
        IForce force;
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

        // 構築した力を加える
        robot.AddForce(force);
    }

    // デバッグ用。使用パーツリストにテストパーツを追加する。
    [ContextMenu("Debug/GetTestParts")]
    private void AddTestParts()
    {
        var data = new PartsInfo.PartsData();
        data.id = PartsPerformance.E_PartsID.TestParts;
        data.angle = 80;
        PartsInfo.Instance.AddParts(data);
    }
}
