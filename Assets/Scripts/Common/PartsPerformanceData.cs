using System.Collections.Generic;
using UnityEngine;

// �A�C�e���̐��\�i�Œ�l�j���`����ScriptableObjects
[CreateAssetMenu(fileName = "PartsDatabase", menuName = "ScriptableObjects/PartsPerformanceDatas")]
public class PartsPerformanceData : ScriptableObject
{
    public List<PartsPerformance> dataList = new List<PartsPerformance>();

    public List<PartsPerformance> getDataList()
    {
        return dataList;
    }

    public PartsPerformance getData(PartsPerformance.E_PartsID id) => dataList.Find(data => data.id == id);
}
