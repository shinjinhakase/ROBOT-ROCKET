using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// �v���C�V�[����ɂ���UI�́A�V�[���J�ڎ��̊J���E���鑀���⏕����Component�B
public class PlayUIManager : MonoBehaviour
{
    [SerializeField] private List<PlayUI> _playUIList = new List<PlayUI>();

    public void UpdateUIState(E_PlaySceneEventType sceneType)
    {
        // �V�[���J�ڂɉ�����UI���܂Ƃ߂ĕ�����J�����肷��
        _playUIList.FindAll(_ui => _ui._openEvents.Contains(sceneType)).ForEach(_ui => _ui._UI?.OpenPanel());
        _playUIList.FindAll(_ui => _ui._closeEvents.Contains(sceneType)).ForEach(_ui => _ui._UI?.ClosePanel());
    }

    // UnityEvent�̃C���X�y�N�^�Ŏg�p����p�̃��\�b�h
    public void UpdateOnStartAnimation() => UpdateUIState(E_PlaySceneEventType.StartAnimation);
    public void UpdateOnStartGame() => UpdateUIState(E_PlaySceneEventType.StartGame);
    public void UpdateOnStartRobotMove() => UpdateUIState(E_PlaySceneEventType.StartRobotMove);
    public void UpdateOnEndGame() => UpdateUIState(E_PlaySceneEventType.EndGame);
    public void UpdateOnGameOver() => UpdateUIState(E_PlaySceneEventType.GameOver);
    public void UpdateOnGameClear() => UpdateUIState(E_PlaySceneEventType.GameClear);
    public void UpdateOnOpenCustomMenu() => UpdateUIState(E_PlaySceneEventType.OpenCustomMenu);
    public void UpdateOnRetry() => UpdateUIState(E_PlaySceneEventType.Retry);
    public void UpdateOnNoReplayRetry() => UpdateUIState(E_PlaySceneEventType.NoReplayRetry);
    public void UpdateOnCheckReplay() => UpdateUIState(E_PlaySceneEventType.CheckReplay);
    public void UpdateOnOpenCustomWhenPlay() => UpdateUIState(E_PlaySceneEventType.OpenCustomWhenPlay);


    // �V�[���̊J���^�C�~���O�����^�C�~���O���̃f�[�^��ێ�����N���X
    [Serializable]
    public class PlayUI
    {
        public UIOpener _UI;
        public List<E_PlaySceneEventType> _openEvents = new List<E_PlaySceneEventType>();
        public List<E_PlaySceneEventType> _closeEvents = new List<E_PlaySceneEventType>();
    }

    // �V�[���̏����^�C�v�̗񋓌^
    public enum E_PlaySceneEventType {
        StartAnimation,
        StartGame,
        StartRobotMove,
        EndGame,
        GameOver,
        GameClear,
        OpenCustomMenu,
        Retry,
        NoReplayRetry,
        CheckReplay,
        OpenCustomWhenPlay
    }
}
