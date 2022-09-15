using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// プレイシーン全体を管理するシングルトン
public class PlaySceneController : SingletonMonoBehaviourInSceneBase<PlaySceneController>
{
    // シーンの処理場面を示す列挙型
    public E_PlayScene scene { get; private set; } = E_PlayScene.FirstCameraMove;
    public int StageNum = -1;

    // カスタムメニューを開けるかの判定
    public bool IsOpenableCustomMenu => scene == E_PlayScene.GamePlay || scene == E_PlayScene.GameEnd;

    [SerializeField] private CameraController cam;
    [SerializeField] private MainRobot robot;

    // ゴールのX座標
    [SerializeField] private float _goalXPoint;
    public float GoalXPoint { get { return _goalXPoint; } private set { _goalXPoint = value; } }
    [SerializeField] private float _score;
    public float Score {
        get { return _score; }
        set { if (value <= _goalXPoint) _score = value; else _score = _goalXPoint; }
    }

    [Header("イベント系統")]
    [Tooltip("ゲーム中にカスタム画面へ以降した際にカスタム画面が開くまでの遅延時間")]
    [SerializeField] private float OpenCustomWhenPlayDuration = 1f;
    [Tooltip("カメラの移動終了後、ゲーム開始直前に呼び出されるメソッド")]
    [SerializeField] private UnityEvent startAnimationEvent = new UnityEvent();
    [Tooltip("ゲーム開始と同時に呼び出されるメソッド")]
    [SerializeField] private UnityEvent startGameEvent = new UnityEvent();
    [Tooltip("ゲーム開始後ロボットが動き始めた際に呼び出されるメソッド")]
    [SerializeField] private UnityEvent startRobotMove = new UnityEvent();
    [Tooltip("ゲーム終了した際、どのような終わり方でも共通して呼び出されるメソッド")]
    [SerializeField] private UnityEvent endGameEvent = new UnityEvent();
    [Tooltip("ゲームオーバーとなった際に呼び出されるメソッド")]
    [SerializeField] private UnityEvent gameOverEvent = new UnityEvent();
    [Tooltip("ゲームクリアした際に呼び出されるメソッド")]
    [SerializeField] private UnityEvent gameClearEvent = new UnityEvent();
    [Tooltip("カスタムメニューを出す際に呼び出されるメソッド")]
    [SerializeField] private UnityEvent OpenCustomMenuEvent = new UnityEvent();
    [Tooltip("現在のリプレイを確認する処理に以降した際に、ステージリセット直前に呼び出される処理")]
    [SerializeField] private UnityEvent CheckReplayEvent = new UnityEvent();

    private IEnumerator hitStopCoroutine;

    private Stage _currentStage = new Stage();
    public Stage CurrentStage { get { return _currentStage; } private set { _currentStage = value; } }


    protected override void Awake()
    {
        base.Awake();

        // カメラ移動の準備をする
        cam.CameraReady();

        // (必要であればここで暗転の解除など)
        InitStageInfo();
    }

    // 最初のカメラ移動が終わった際に呼び出されるメソッド
    [ContextMenu("Scene/EndFirstCameraMove")]
    public void endFirstCameraMove()
    {
        if (scene == E_PlayScene.FirstCameraMove)
        {
            scene = E_PlayScene.StartAnimation;
            _score = 0;

            // 開始アニメーション処理を呼び出す
            startAnimationEvent.Invoke();
        }
    }
    // ゲームが開始した際に呼び出されるメソッド
    [ContextMenu("Scene/GameStart")]
    public void GameStart()
    {
        if (scene == E_PlayScene.StartAnimation)
        {
            scene = E_PlayScene.GamePlay;

            // 開始時のイベントを呼び出す
            startGameEvent.Invoke();

            // TODO：ゲーム開始処理（シャドウに開始を伝えるなどの色々な処理）
            robot.GameStart();
        }
    }
    // ロボットが動き始めた際に呼び出されるメソッド
    public void RobotStartMove()
    {
        if (scene == E_PlayScene.GamePlay)
        {
            startRobotMove.Invoke();
        }
    }
    // カスタムメニューを開く
    [ContextMenu("Scene/OpenCustomMenu")]
    public void OpenCustomMenu()
    {
        if (IsOpenableCustomMenu)
        {
            bool IsNeedSetResult = false;
            if (scene == E_PlayScene.GamePlay)
            {
                IsNeedSetResult = true;
            }
            scene = E_PlayScene.CustomMenu;

            // 飛行中なら、ロボットを連れていってカスタムメニューを開く処理に移る
            cam.IsFollowRobot = false;
            robot.OpenCustomMenu();

            if (IsNeedSetResult) endGameEvent.Invoke();

            // パーツの状態をカスタム時に戻す
            PartsInfo.Instance.Reset();

            // カスタムメニューのオープン処理
            if (IsNeedSetResult && robot._status.IsFlying) {
                CallMethodAfterDuration(OpenCustomMenuEvent.Invoke, OpenCustomWhenPlayDuration);
            }
            else
            {
                OpenCustomMenuEvent.Invoke();
            }
        }
    }
    // カスタムメニューを閉じたときの処理
    [ContextMenu("Scene/CloseCustomMenu")]
    public void CloseCustomMenu()
    {
        if (scene == E_PlayScene.CustomMenu) 
        {
            scene = E_PlayScene.FirstCameraMove;

            // ステージをリセットし、最初からやり直す。
            ResetStage();

            // TODO：カメラの調整などの処理
            cam.IsFollowRobot = true;

            endFirstCameraMove();
        }
    }
    // ゲームクリア処理を行う
    [ContextMenu("Scene/GameClear")]
    public void GameClear()
    {
        if(scene == E_PlayScene.GamePlay)
        {
            scene = E_PlayScene.GameEnd;

            cam.IsFollowRobot = false;
            robot.GameClear();

            endGameEvent.Invoke();

            gameClearEvent.Invoke();
        }
    }
    // ゲームオーバー処理を行う
    [ContextMenu("Scene/GameOver")]
    public void GameOver()
    {
        if (scene == E_PlayScene.GamePlay)
        {
            scene = E_PlayScene.GameEnd;

            // カメラの追尾を切り、ロボットのゲームオーバー処理を実行する
            cam.IsFollowRobot = false;
            robot.GameOver();

            endGameEvent.Invoke();
            
            // ロボットパージアニメーション待機後に、結果表示をするなどの処理を呼ぶ
            gameOverEvent.Invoke();
        }
    }
    // 現在のプレイをリプレイで確認する
    [ContextMenu("Scene/CheckReplay")]
    public void CheckReplay()
    {
        if (scene == E_PlayScene.GameEnd)
        {
            scene = E_PlayScene.FirstCameraMove;

            // ロボットをリプレイモードに設定する
            CheckReplayEvent.Invoke();
            robot.SetReplayMode();

            // ステージをリセットし、最初からやり直す。
            ResetStage();

            // TODO：カメラの調整などの処理
            cam.IsFollowRobot = true;

            Invoke("endFirstCameraMove", 0.1f);
        }
    }
    // リセットの処理を行う
    [ContextMenu("Debug/ResetStage")]
    public void ResetStage()
    {
        // ヒットストップ処理を停止し、時間の流れを戻す
        StopHitStopIfExists(true);

        // 使用パーツ状況とメインロボット状況をリセット
        PlayPartsManager.Instance.ResetPartsStatus();
        robot.ResetToStart();
        // カメラをロボットの所へ初期状態でセット
        cam.SetCameraToRobot();

        // TODO：シャドウの初期化処理
        // TODO：ステージの復元処理（稼働オブジェクトがあるなら）
    }


    // 一時停止する形でヒットストップを実装する
    public void RequestHitStopByStop(float time)
    {
        if (time <= 0) return;
        SetHitStop(0, time);
    }

    // スローになる形でヒットストップを実装する
    public void RequestHitStopBySlow(float timeScale, float time)
    {
        if (time <= 0 || timeScale < 0) return;
        SetHitStop(timeScale, time);
    }



    // 新たに指定の秒数だけ時間経過速度を変更する
    private void SetHitStop(float timeScale, float time)
    {
        // 実行していたヒットストップ処理を中断し、新たにヒットストップ処理を始める
        StopHitStopIfExists(false);
        hitStopCoroutine = ChangeTimeScale(timeScale, time);
        StartCoroutine(hitStopCoroutine);
    }
    // 実行しているヒットストップがあるなら、その処理を中断して止める
    public void StopHitStopIfExists(bool ResetTimeScale)
    {
        if (hitStopCoroutine != null)
        {
            StopCoroutine(hitStopCoroutine);
            hitStopCoroutine = null;
        }
        // ResetTimeScaleがtrueなら、時間の速度を元に戻す
        if (ResetTimeScale) Time.timeScale = 1f;
    }

    // 指定の秒数TimeScaleを変化させるメソッド
    private IEnumerator ChangeTimeScale(float timeScale, float time)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }
    // 指定のメソッドを指定時間後に呼び出すメソッド
    private void CallMethodAfterDuration(Action action, float time)
    {
        StartCoroutine(CallMethodAfterDurationEnumerator(action, time));
    }
    private IEnumerator CallMethodAfterDurationEnumerator(Action action, float time)
    {
        if (time > 0) yield return new WaitForSeconds(time);
        action();
        yield break;
    }
    // ステージ情報を受け取るメソッド
    private void InitStageInfo()
    {
        StageSelectGlobal _stageSelectGlobal = StageSelectGlobal.Instance;
        if (_stageSelectGlobal)
        {
            CurrentStage = _stageSelectGlobal.Stage;
            StageNum = CurrentStage.StageNum;
            /* ゴール座標はどうするか */
        }
    }

    // シーンの処理場面を示す列挙型
    public enum E_PlayScene
    {
        FirstCameraMove,    // 最初のゴールからロボットまでのカメラの移動
        StartAnimation,     // 開始アニメーション
        GamePlay,           // ゲーム進行中
        GameEnd,            // ゲーム終了
        CustomMenu          // カスタムメニューを開いている
    }
}
