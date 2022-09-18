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

    // ����̗͂̎�ނ̃p�[�cID���擾����
    public List<PartsPerformance.E_PartsID> getIDByForceType(PartsPerformance.E_ForceType forceType)
    {
        return dataList.FindAll(data => data.forceType == forceType).ConvertAll(data => data.id);
    }
}
