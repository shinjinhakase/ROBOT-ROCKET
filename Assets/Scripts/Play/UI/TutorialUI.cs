using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// チュートリアル用のUIウィンドウの表示切替操作Component。
public class TutorialUI : UIOpener
{
    // 文章を表示するテキスト
    [SerializeField] private Text _text;

    // 表示設定
    [Header("表示設定")]
    [SerializeField] private int _tutorialGroupNo;
    [Tooltip("これをオンにすると、このUIを閉じた際にチュートリアル完了と見なされ、以降は同じチュートリアルのUIが表示されなくなる。")]
    [SerializeField] private bool IsFinalTutorialUI = false;

    private int _nowPageIndex = -1;
    [SerializeField] private List<TutorialUIPageData> _pages = new List<TutorialUIPageData>();

    public override void OpenPanel()
    {
        var _tutorialManager = TutorialManager.Instance;
        if (!_tutorialManager.IsEndTutorial(_tutorialGroupNo) && _tutorialManager.CheckRequireData(_tutorialGroupNo) && !PlaySceneController.Instance.IsReplayMode && _pages?.Count > 0)
        {
            base.OpenPanel();

            // 表示するページを1ページ目に切り替える
            ChangePage(0);
        }
    }

    // 次へ進むボタンの処理（表示を切り替える、もしくはUIを閉じる処理）
    public void NextButton()
    {
        if (!gameObject.activeSelf) return;
        if (_nowPageIndex == _pages.Count - 1) ClosePanel();
        else ChangePage(_nowPageIndex + 1);
    }

    // 表示しているページを切り替える処理
    private void ChangePage(int pageIndex)
    {
        // 現在表示しているUIのデータと次に表示するUIのデータの取得
        if (pageIndex < 0 || pageIndex >= _pages.Count) throw new ArgumentOutOfRangeException(nameof(pageIndex));
        else if (pageIndex == _nowPageIndex) return;
        TutorialUIPageData nowPageData = _nowPageIndex >= 0 && _nowPageIndex < _pages.Count ? _pages[_nowPageIndex] : null;
        TutorialUIPageData nextPageData = _pages[pageIndex];

        // テキスト・UIの表示を切り替える
        if (_text != null) _text.text = nextPageData._text;
        nowPageData?._UIList.ForEach(UI => UI.ClosePanel());
        nextPageData._UIList.ForEach(_UI => _UI.OpenPanel());
        _nowPageIndex = pageIndex;
    }

    public override void ClosePanel()
    {
        if (gameObject.activeSelf && IsFinalTutorialUI) TutorialManager.Instance.SetEndTutorial(_tutorialGroupNo);
        base.ClosePanel();
    }

    // ページの表示設定を所持するクラス
    [Serializable]
    public class TutorialUIPageData
    {
        [TextArea(3, 5)] public string _text;
        public List<UIOpener> _UIList = new List<UIOpener>();
    }
}
