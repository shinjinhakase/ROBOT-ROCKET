using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定期的にオン・オフを切り替えるギミック
public class IntervalFlickerGimick : IntervalActionGimick
{
    [Tooltip("オン・オフを切り替える対象。自身を選択せず、子オブジェクトなどを選ぶこと。")]
    [SerializeField] private GameObject target;
    [Tooltip("ゲームが開始した際の初期状態")]
    [SerializeField] private bool InitialState;

    // シーンが開始した際に呼ばれるメソッド
    public override void OnSceneStart() {
        target.SetActive(InitialState);
    }

    // ギミックをリセットするメソッド
    public override void ResetGimick()
    {
        base.ResetGimick();
        target.SetActive(InitialState);
    }

    // 定期的に呼び出されるアクションメソッド
    protected override void Action()
    {
        base.Action();
        target.SetActive(!target.activeSelf);
    }

}
