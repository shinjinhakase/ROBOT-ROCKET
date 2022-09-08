using System.Collections.Generic;
using UnityEngine;

// アイテムの性能（固定値）を定義するScriptableObjects
[CreateAssetMenu(fileName = "PartsDatabase", menuName = "ScriptableObjects/PartsPerformanceDatas")]
public class PartsPerformanceData : ScriptableObject
{
    public List<PartsPerformance> dataList = new List<PartsPerformance>();

    public List<PartsPerformance> getDataList()
    {
        return dataList;
    }

    public PartsPerformance getData(PartsPerformance.E_PartsID id) => dataList.Find(data => data.id == id);

    // 特定の力の種類のパーツIDを取得する
    public List<PartsPerformance.E_PartsID> getIDByForceType(PartsPerformance.E_ForceType forceType)
    {
        return dataList.FindAll(data => data.forceType == forceType).ConvertAll(data => data.id);
    }
}
