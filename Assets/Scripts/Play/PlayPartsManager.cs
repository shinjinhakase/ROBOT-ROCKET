using System;
using UnityEngine;
using UnityEngine.Events;

// プレイシーンで使用するアイテムを管理するComponent。
// 道中でのアイテムの入手・使用、重量計算はこっちを介する。
public class PlayPartsManager : SingletonMonoBehaviourInSceneBase<PlayPartsManager>
{
    // 現在パーツ使用中かのフラグ
    [NonSerialized] public bool IsUsingParts = false;

    private PartsInfo partsInfo;
    [SerializeField] private PartsPerformanceData partsPerformanceData;

    [SerializeField] private UnityEvent<PartsInfo.PartsData> getPartsEvent = new UnityEvent<PartsInfo.PartsData>();

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

    // パーツIDからアイテム性能を返す
    public PartsPerformance GetPerformance(PartsPerformance.E_PartsID id) => partsPerformanceData.getData(id);

    // パーツを使う（使うパーツのデータと、生まれる力を返す）
    public void UseParts(out PartsPerformance performance, out PartsInfo.PartsData data, out IForce force)
    {
        // 使用するパーツのデータを取得する
        if (partsInfo.Length == 0) throw new Exception("使用するパーツがありません。");
        IsUsingParts = true;
        data = partsInfo.GetParts(0);
        performance = partsPerformanceData.getData(data.id);

        // 加える力を構築する
        switch (performance.forceType)
        {
            case PartsPerformance.E_ForceType.Bomb:
                force = new ImpulseForce(data.angle, performance.F, performance.m, true);
                break;
            case PartsPerformance.E_ForceType.Rocket:
            case PartsPerformance.E_ForceType.Propeller:
                force = new PressForce(data.angle, performance.F, performance.t, performance.k, performance.m, true);
                break;
            case PartsPerformance.E_ForceType.NoForce:
                force = null;
                break;
            case PartsPerformance.E_ForceType.Glider:
                force = new GliderForce(data.angle, performance.F, performance.t, performance.R, performance.m, true);
                break;
            case PartsPerformance.E_ForceType.CollisionForce:
            default:
                throw new Exception("ロボットに加える力を構築できません。");
        }
    }

    // パーツを獲得する処理
    public void GetParts(PartsInfo.PartsData data, out PartsPerformance performance)
    {
        partsInfo.AddParts(data);
        performance = partsPerformanceData.getData(data.id);
        getPartsEvent.Invoke(data);

        Debug.Log("パーツを獲得しました：ID = " + data.id);
    }
    public void GetParts(PartsInfo.PartsData data)
    {
        GetParts(data, out _);
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

    // パーツの使用状況をリセットする。
    public void ResetPartsStatus()
    {
        // アイテムの使用状況をリセットし、カスタムパーツリストを保存した状態に戻す。
        IsUsingParts = false;
        partsInfo.Reset();
        partsInfo = PartsInfo.Instance;

        Debug.Log("カスタムパーツデータを保存時の状態にリセットしました");
    }
}
