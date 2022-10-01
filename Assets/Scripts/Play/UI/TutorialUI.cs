using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �`���[�g���A���p��UI�E�B���h�E�̕\���֑ؑ���Component�B
public class TutorialUI : UIOpener
{
    // ���͂�\������e�L�X�g
    [SerializeField] private Text _text;

    // �\���ݒ�
    [Header("�\���ݒ�")]
    [SerializeField] private int _tutorialGroupNo;
    [Tooltip("������I���ɂ���ƁA����UI������ۂɃ`���[�g���A�������ƌ��Ȃ���A�ȍ~�͓����`���[�g���A����UI���\������Ȃ��Ȃ�B")]
    [SerializeField] private bool IsFinalTutorialUI = false;

    private int _nowPageIndex = -1;
    [SerializeField] private List<TutorialUIPageData> _pages = new List<TutorialUIPageData>();

    public override void OpenPanel()
    {
        var _tutorialManager = TutorialManager.Instance;
        if (!_tutorialManager.IsEndTutorial(_tutorialGroupNo) && _tutorialManager.CheckRequireData(_tutorialGroupNo) && !PlaySceneController.Instance.IsReplayMode && _pages?.Count > 0)
        {
            base.OpenPanel();

            // �\������y�[�W��1�y�[�W�ڂɐ؂�ւ���
            ChangePage(0);
        }
    }

    // ���֐i�ރ{�^���̏����i�\����؂�ւ���A��������UI����鏈���j
    public void NextButton()
    {
        if (!gameObject.activeSelf) return;
        if (_nowPageIndex == _pages.Count - 1) ClosePanel();
        else ChangePage(_nowPageIndex + 1);
    }

    // �\�����Ă���y�[�W��؂�ւ��鏈��
    private void ChangePage(int pageIndex)
    {
        // ���ݕ\�����Ă���UI�̃f�[�^�Ǝ��ɕ\������UI�̃f�[�^�̎擾
        if (pageIndex < 0 || pageIndex >= _pages.Count) throw new ArgumentOutOfRangeException(nameof(pageIndex));
        else if (pageIndex == _nowPageIndex) return;
        TutorialUIPageData nowPageData = _nowPageIndex >= 0 && _nowPageIndex < _pages.Count ? _pages[_nowPageIndex] : null;
        TutorialUIPageData nextPageData = _pages[pageIndex];

        // �e�L�X�g�EUI�̕\����؂�ւ���
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

    // �y�[�W�̕\���ݒ����������N���X
    [Serializable]
    public class TutorialUIPageData
    {
        [TextArea(3, 5)] public string _text;
        public List<UIOpener> _UIList = new List<UIOpener>();
    }
}
