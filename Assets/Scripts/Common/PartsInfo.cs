using System;
using System.Collections.Generic;

// �p�[�c�̏��̓��A�J�X�^���Ɋւ������i�[���Ă���N���X�BPartsInfo.Instance�ŃA�N�Z�X�\�B
[Serializable]
public class PartsInfo : SavableSingletonBase<PartsInfo>
{
    // �g�p�ҋ@���Ă���p�[�c�̃��X�g�i�g�p���ɕ��ׂ�j
    public List<PartsData> partsList = new List<PartsData>();

    //�����X�g�̑�������\�b�h�ɋN�����Ă����B�g�����������炲���R�ɂǂ���
    public int Length => partsList.Count;   // �p�[�c�̐����擾
    public bool HasNext => partsList.Count > 0; // �g�p����p�[�c�����邩
    public List<PartsData> GetPartsList() => partsList; // �p�[�c�̃��X�g���擾
    public PartsData GetParts(int index) => index >= partsList.Count ? null : partsList[index]; // �w��̏��Ԃ̂Ƃ��Ɏg�p����p�[�c���擾
    // �w��̃p�[�c�̏��Ԃ����ւ���
    public void SwitchPartsOrder(int index1, int index2)
    {
        if (index1 >= partsList.Count || index2 >= partsList.Count) return;
        PartsData tmp = partsList[index1];
        partsList[index1] = partsList[index2];
        partsList[index2] = tmp;
    }
    public void RemoveParts(int index = 0) => partsList.RemoveAt(index);
    public void AddParts(PartsData data) => partsList.Add(data);


    // �p�[�c��̃f�[�^���i�[���Ă���N���X
    [Serializable]
    public class PartsData
    {
        public PartsPerformance.E_PartsID id;
        public float angle;
    }
}
