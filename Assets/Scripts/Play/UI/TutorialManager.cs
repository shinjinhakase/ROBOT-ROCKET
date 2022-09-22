using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �`���[�g���A���̐i�s�󋵊Ǘ��V���O���g��
public class TutorialManager : SingletonMonoBehaviourInSceneBase<TutorialManager>
{
    // �`���[�g���A�����J���̂Ɍ��Ă����K�v������ʂ̃`���[�g���A���̐ݒ�
    [Tooltip("���̃`���[�g���A�����n�߂�̂ɃN���A���Ă����K�v������ʂ̃`���[�g���A���ԍ��ݒ�")]
    [SerializeField] private List<RequireTutorialData> requireTutorialDataList = new List<RequireTutorialData>();
    public bool CheckRequireData(int TutorialNo) => requireTutorialDataList.Exists(data => data.TutorialNo == TutorialNo) ? requireTutorialDataList.Find(data => data.TutorialNo == TutorialNo).RequireTutorialList?.TrueForAll(no => IsEndTutorial(no)) ?? true : true;

    // �`���[�g���A�����I���������̃t���O�Ǘ�
    private List<int> _endTutorialFlags = new List<int>();
    public void ClearTuturialFlags() => _endTutorialFlags.Clear();
    public void SetEndTutorial(int TutorialNo)
    {
        if (!_endTutorialFlags.Contains(TutorialNo)) _endTutorialFlags.Add(TutorialNo);
    }
    public bool IsEndTutorial(int TutorialNo) => _endTutorialFlags.Contains(TutorialNo);


    // �v���`���[�g���A���ݒ�N���X
    [Serializable]
    public struct RequireTutorialData {
        [Tooltip("�ǂ̏����ɂ��g���܂���B������₷���悤�ɖ����������Ă����p�ł��B")]
        public string _text;
        public int TutorialNo;
        public List<int> RequireTutorialList;
    }
}
