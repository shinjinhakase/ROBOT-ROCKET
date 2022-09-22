using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// プレイシーン上にあるUIの、シーン遷移時の開く・閉じる操作を補助するComponent。
public class PlayUIManager : MonoBehaviour
{
    [SerializeField] private List<PlayUI> _playUIList = new List<PlayUI>();

    public void UpdateUIState(E_PlaySceneEventType sceneType)
    {
        // シーン遷移に応じてUIをまとめて閉じたり開いたりする
        _playUIList.FindAll(_ui => _ui._openEvents.Contains(sceneType)).ForEach(_ui => _ui._UI?.OpenPanel());
        _playUIList.FindAll(_ui => _ui._closeEvents.Contains(sceneType)).ForEach(_ui => _ui._UI?.ClosePanel());
    }

    // UnityEventのインスペクタで使用する用のメソッド
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


    // シーンの開くタイミングや閉じるタイミング等のデータを保持するクラス
    [Serializable]
    public class PlayUI
    {
        public UIOpener _UI;
        public List<E_PlaySceneEventType> _openEvents = new List<E_PlaySceneEventType>();
        public List<E_PlaySceneEventType> _closeEvents = new List<E_PlaySceneEventType>();
    }

    // シーンの処理タイプの列挙型
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
