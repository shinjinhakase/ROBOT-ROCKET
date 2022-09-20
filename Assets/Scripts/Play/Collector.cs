using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 収集アイテムの個数を管理するシングルトン
public class Collector : SingletonMonoBehaviourInSceneBase<Collector>
{
    // ステージに配置してある個数を数えるリスト
    private CollectionDataList _stageCollectionDatas = CollectionDataList.Build();
    public CollectionDataList StageCollectionDatas => _stageCollectionDatas.Clone();
    // 個数を管理するリスト
    private CollectionDataList _getCollectionDatas = CollectionDataList.Build();
    public CollectionDataList GetCollectionDatas => _getCollectionDatas.Clone();


    // 獲得したアイテム数を初期化する
    public void ResetGetNum()
    {
        _getCollectionDatas.Clear();
    }

    // ステージにあるアイテムの処理
    public void RegisterItem(CollectionDrop.E_CollectionID id, int num = 1)
    {
        _stageCollectionDatas.AddNum(id, num);
    }

    // アイテムを獲得した際の処理
    public void GetItem(CollectionDrop.E_CollectionID id, int num = 1)
    {
        _getCollectionDatas.AddNum(id, num);
    }



    // 収集したアイテムの個数データ
    [Serializable]
    public struct CollectionData
    {
        public CollectionDrop.E_CollectionID id;    // ID
        public int num; // 個数

        public CollectionData(CollectionDrop.E_CollectionID id)
        {
            this.id = id;
            num = 0;
        }
    }

    // 収集したアイテムの個数データリスト
    [Serializable]
    public struct CollectionDataList
    {
        [SerializeField] private List<CollectionData> datas;

        // 初期化
        public static CollectionDataList Build()
        {
            var datalist = new CollectionDataList();
            datalist.datas = new List<CollectionData>();
            return datalist;
        }

        // コピーコンストラクタ
        public CollectionDataList(CollectionDataList datalist)
        {
            datas = new List<CollectionData>(datalist.datas);
        }
        public CollectionDataList Clone() => new CollectionDataList(this);

        public void Clear() { datas.Clear(); }
        public CollectionData GetData(CollectionDrop.E_CollectionID id) => datas.Exists(data => data.id == id) ? datas.Find(data => data.id == id) : new CollectionData(id);
        private void SetData(CollectionData newData)
        {
            datas.RemoveAll(data => data.id == newData.id);
            datas.Add(newData);
        }

        // 個数を増やす
        public void AddNum(CollectionDrop.E_CollectionID id, int num = 1)
        {
            var data = GetData(id);
            data.num += num;
            SetData(data);
        }
    }
}
