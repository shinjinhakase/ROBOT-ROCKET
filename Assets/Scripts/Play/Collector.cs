using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ���W�A�C�e���̌����Ǘ�����V���O���g��
public class Collector : SingletonMonoBehaviourInSceneBase<Collector>
{
    // �X�e�[�W�ɔz�u���Ă�����𐔂��郊�X�g
    private CollectionDataList _stageCollectionDatas = CollectionDataList.Build();
    public CollectionDataList StageCollectionDatas => _stageCollectionDatas.Clone();
    // �����Ǘ����郊�X�g
    private CollectionDataList _getCollectionDatas = CollectionDataList.Build();
    public CollectionDataList GetCollectionDatas => _getCollectionDatas.Clone();

    // �A�C�e���̊l�����ɌĂяo�����C�x���g
    [SerializeField] private List<CollectionGetEvent> _collectionGetEvent = new List<CollectionGetEvent>();

    // �l�������A�C�e����������������
    public void ResetGetNum()
    {
        _getCollectionDatas.Clear();
    }

    // �l�������A�C�e���������v���C�f�[�^���ǂݍ���
    public void LoadFromReplayData(ReplayData data)
    {
        _getCollectionDatas = data.getCollectionDatas.Clone();
    }

    // �X�e�[�W�ɂ���A�C�e���̏���
    public void RegisterItem(CollectionDrop.E_CollectionID id, int num = 1)
    {
        _stageCollectionDatas.AddNum(id, num);
    }

    // �A�C�e�����l�������ۂ̏���
    public void GetItem(CollectionDrop.E_CollectionID id, int num = 1)
    {
        _getCollectionDatas.AddNum(id, num);
        _collectionGetEvent.ForEach(item => item.InvokeIfMatchID(id));
    }



    // ���W�����A�C�e���̌��f�[�^
    [Serializable]
    public struct CollectionData
    {
        public CollectionDrop.E_CollectionID id;    // ID
        public int num; // ��

        public CollectionData(CollectionDrop.E_CollectionID id)
        {
            this.id = id;
            num = 0;
        }
    }

    // ���W�����A�C�e���̌��f�[�^���X�g
    [Serializable]
    public struct CollectionDataList
    {
        [SerializeField] private List<CollectionData> datas;

        // ������
        public static CollectionDataList Build()
        {
            var datalist = new CollectionDataList();
            datalist.datas = new List<CollectionData>();
            return datalist;
        }

        // �R�s�[�R���X�g���N�^
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

        // ���𑝂₷
        public void AddNum(CollectionDrop.E_CollectionID id, int num = 1)
        {
            var data = GetData(id);
            data.num += num;
            SetData(data);
        }
    }

    [Serializable]
    private class CollectionGetEvent
    {
        public CollectionDrop.E_CollectionID _collectionID;
        public UnityEvent _events = new UnityEvent();

        public void InvokeIfMatchID(CollectionDrop.E_CollectionID id)
        {
            if (_collectionID == id) _events.Invoke();
        }
    }
}
