using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// プレイシーン全体を管理するシングルトン
public class PlaySceneController : SingletonMonoBehaviourInSceneBase<PlaySceneController>
{
    // シーンの処理場面を示す列挙型
    public E_PlayScene scene { get; private set; } = E_PlayScene.FirstCameraMove;

    [SerializeField] private CameraController cam;
    [SerializeField] private MainRobot robot;


    // ゴールのX座標
    [SerializeField] private float _goalXPoint;
    public float GoalXPoint
    {
        get { return _goalXPoint; }
        private set { _goalXPoint = value; }
    }

    [Tooltip("カメラの移動終了後、ゲーム開始直前に呼び出されるメソッド")]
    [SerializeField] private UnityEvent startAnimation = new UnityEvent();

    private IEnumerator hitStopCoroutine;


    protected override void Awake()
    {
        base.Awake();

        // カメラ移動の準備をする
        // (必要であれば暗転の解除など)
        cam.CameraReady();
    }

    // 最初のカメラ移動が終わった際に呼び出されるメソッド
    public void endFirstCameraMove()
    {
        if (scene == E_PlayScene.FirstCameraMove)
        {
            scene = E_PlayScene.StartAnimation;

            // 開始アニメーション処理を呼び出す
            startAnimation.Invoke();
        }
    }
    // ゲームが開始した際に呼び出されるメソッド
    public void GameStart()
    {
        if (scene == E_PlayScene.StartAnimation)
        {
            scene = E_PlayScene.GamePlay;

            // TODO：ゲーム開始処理（シャドウに開始を伝えるなどの色々な処理）
            robot.GameStart();
        }
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
        if(hitStopCoroutine != null)
        {
            StopCoroutine(hitStopCoroutine);
            hitStopCoroutine = null;
        }
        hitStopCoroutine = ChangeTimeScale(timeScale, time);
        StartCoroutine(hitStopCoroutine);
    }

    // 指定の秒数TimeScaleを変化させるメソッド
    private IEnumerator ChangeTimeScale(float timeScale, float time)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }

    // シーンの処理場面を示す列挙型
    public enum E_PlayScene
    {
        FirstCameraMove,    // 最初のゴールからロボットまでのカメラの移動
        StartAnimation,     // 開始アニメーション
        GamePlay,           // ゲーム進行中
        GameEnd             // ゲーム終了
    }
}
