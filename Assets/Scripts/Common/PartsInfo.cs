using System;
using System.Collections.Generic;

// パーツの情報の内、カスタムに関わる情報を格納しているクラス。PartsInfo.Instanceでアクセス可能。
[Serializable]
public class PartsInfo : SavableSingletonBase<PartsInfo>
{
    // 使用待機しているパーツのリスト（使用順に並べる）
    public List<PartsData> partsList = new List<PartsData>();

    //↓リストの操作をメソッドに起こしておく。使いたかったらご自由にどうぞ
    public int Length => partsList.Count;   // パーツの数を取得
    public bool HasNext => partsList.Count > 0; // 使用するパーツがあるか
    public List<PartsData> GetPartsList() => partsList; // パーツのリストを取得
    public PartsData GetParts(int index) => index >= partsList.Count ? null : partsList[index]; // 指定の順番のときに使用するパーツを取得
    // 指定のパーツの順番を入れ替える
    public void SwitchPartsOrder(int index1, int index2)
    {
        if (index1 >= partsList.Count || index2 >= partsList.Count) return;
        PartsData tmp = partsList[index1];
        partsList[index1] = partsList[index2];
        partsList[index2] = tmp;
    }
    public void RemoveParts(int index = 0) => partsList.RemoveAt(index);
    public void AddParts(PartsData data) => partsList.Add(data);


    // パーツ一つのデータを格納しているクラス
    [Serializable]
    public class PartsData
    {
        public PartsPerformance.E_PartsID id;
        public float angle;
    }
}
