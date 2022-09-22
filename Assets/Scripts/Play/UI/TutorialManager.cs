using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// チュートリアルの進行状況管理シングルトン
public class TutorialManager : SingletonMonoBehaviourInSceneBase<TutorialManager>
{
    // チュートリアルを開くのに見ておく必要がある別のチュートリアルの設定
    [Tooltip("このチュートリアルを始めるのにクリアしておく必要がある別のチュートリアル番号設定")]
    [SerializeField] private List<RequireTutorialData> requireTutorialDataList = new List<RequireTutorialData>();
    public bool CheckRequireData(int TutorialNo) => requireTutorialDataList.Exists(data => data.TutorialNo == TutorialNo) ? requireTutorialDataList.Find(data => data.TutorialNo == TutorialNo).RequireTutorialList?.TrueForAll(no => IsEndTutorial(no)) ?? true : true;

    // チュートリアルが終了したかのフラグ管理
    private List<int> _endTutorialFlags = new List<int>();
    public void ClearTuturialFlags() => _endTutorialFlags.Clear();
    public void SetEndTutorial(int TutorialNo)
    {
        if (!_endTutorialFlags.Contains(TutorialNo)) _endTutorialFlags.Add(TutorialNo);
    }
    public bool IsEndTutorial(int TutorialNo) => _endTutorialFlags.Contains(TutorialNo);


    // 要求チュートリアル設定クラス
    [Serializable]
    public struct RequireTutorialData {
        [Tooltip("どの処理にも使われません。分かりやすいように命名だけしておく用です。")]
        public string _text;
        public int TutorialNo;
        public List<int> RequireTutorialList;
    }
}
