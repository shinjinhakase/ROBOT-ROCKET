using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイシーン全体を管理するシングルトン
public class PlaySceneController : SingletonMonoBehaviourInSceneBase<PlaySceneController>
{
    IEnumerator hitStopCoroutine;


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
}
