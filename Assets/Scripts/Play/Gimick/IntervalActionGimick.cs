using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 定期的にアクションを起こすギミックの基底Component。
public class IntervalActionGimick : GimickBase
{
    // 待機秒数
    [SerializeField] protected float _initWait; // 開始直後の待機秒数
    [SerializeField] protected float _interval; // インターバル（秒）

    // 定期的に呼び出されるアクションイベント
    [SerializeField] private UnityEvent _actionEvent = new UnityEvent();

    private IEnumerator intervalCaller;

    // ロボットが動き始めた際に動きを同期するメソッド
    public override void OnStartRobot() {
        if (intervalCaller != null)
        {
            StopCoroutine(intervalCaller);
            intervalCaller = null;
        }
        intervalCaller = IntervalActionCall();
        StartCoroutine(intervalCaller);
    }

    // ギミックをリセットするメソッド
    public override void ResetGimick() {
        if (intervalCaller != null) {
            StopCoroutine(intervalCaller);
            intervalCaller = null;
        }
    }

    // 定期的に呼び出されるアクションメソッド
    protected virtual void Action() {
        _actionEvent.Invoke();
    }

    // 定期的にアクションを呼び出すメソッド
    private IEnumerator IntervalActionCall()
    {
        if (_initWait > 0) yield return new WaitForSeconds(_initWait);

        WaitForSeconds waitForSeconds = new WaitForSeconds(_interval);
        while (true)
        {
            Action();
            yield return waitForSeconds;
        }
    }
}
