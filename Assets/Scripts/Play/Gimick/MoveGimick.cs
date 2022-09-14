using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 周期的な直線移動をするギミックのComponent。
public class MoveGimick : GimickBase
{
    [SerializeField] private List<MoveTransformData> moveTransformDatas = new List<MoveTransformData>();
    private int Length => moveTransformDatas.Count;

    private bool IsStarted = false;
    private Vector2 homePosition;       // 初期位置
    private Vector2 subHomePosition;    // 動き始めの位置
    private Vector2 direction;          // 動く方向
    private int index = 0;      // 参照インデックス
    private int lastFrame = 0;  // 終了フレーム
    private int frameCnt = 0;   // フレームカウント

    private void FixedUpdate()
    {
        if (IsStarted)
        {
            // データ更新
            if (frameCnt == 0)
            {
                // 位置を更新
                MoveTransformData nowData = moveTransformDatas[index];
                MoveTransformData nextData = moveTransformDatas[(index + 1) % Length];
                subHomePosition = homePosition + nowData._position;
                direction = nextData._position - nowData._position;

                // 最終フレームを計算
                lastFrame = Mathf.RoundToInt(nowData._time / Time.fixedDeltaTime);
                if (lastFrame <= 0) lastFrame = 1;
            }

            // 座標を更新
            transform.position = subHomePosition + direction * frameCnt / lastFrame;

            // データ更新判定
            frameCnt++;
            if (frameCnt >= lastFrame)
            {
                index = (index + 1) % Length;
                frameCnt = 0;
            }
        }
    }

    // シーンが開始した際に呼ばれるメソッド
    public override void OnSceneStart() {
        homePosition = new Vector3(transform.position.x, transform.position.y);
        if (Length > 0) transform.position = homePosition + moveTransformDatas[0]._position;
    }

    // ロボットが動き始めた際に動きを同期するメソッド
    public override void OnStartRobot() {
        index = 0;
        frameCnt = 0;
        if (Length > 1) IsStarted = true;
    }

    // ギミックをリセットするメソッド
    public override void ResetGimick() {
        IsStarted = false;
        if (Length > 1) transform.position = homePosition + moveTransformDatas[0]._position;
    }

    [Serializable]
    private class MoveTransformData
    {
        [Tooltip("初期位置からの相対座標")]
        public Vector2 _position;
        [Tooltip("今の位置から次の位置に移動するのに掛ける時間")]
        public float _time;
    }
}
