using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ロボットの状態を管理するクラス。アニメーションなどを処理したりする。
// 操作キャラ・リプレイ・シャドウの基盤。
public class RobotStatus : MonoBehaviour
{
    // ロボットの状態を示す列挙型
    private enum E_RobotStatus
    {
        Ready,      // ゲーム開始前の待機状態
        Fly,        // 飛行状態（通常の待機状態）
        UseParts,   // アイテム使用中
        Cooldown,   // クールタイム待機中
        EndFly,     // 飛行終了（ゲームクリアやゲームオーバーなど）
    }
    private E_RobotStatus _status = E_RobotStatus.Ready;
    private int cooltime = 0;       // クールタイム

    // ロボットの状態判定メソッド
    public bool IsPartsUsable => _status == E_RobotStatus.Fly;  // 装備パーツの使用可能判定
    public bool IsUsingParts => _status == E_RobotStatus.UseParts;  // パーツの使用中判定
    public bool IsFlying => _status != E_RobotStatus.Ready && _status != E_RobotStatus.EndFly;  // 飛行中判定（ゲーム中判定）
    public bool IsEndFly => _status == E_RobotStatus.EndFly;    // 飛行終了判定


    [SerializeField] private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // クールタイム消費
        if (IsFlying && cooltime > 0)
        {
            cooltime--;
            if (cooltime <= 0)
            {
                endCooltime();
            }
        }
    }

    // ゲーム開始時に呼ぶメソッド
    public void startGame()
    {
        if(_status != E_RobotStatus.Ready)
        {
            Debug.LogWarning("ゲーム開始前以外にゲーム開始メソッドが呼ばれました。");
            return;
        }

        _status = E_RobotStatus.Fly;

        // TODO：飛行のアニメーションへ遷移
    }

    // パーツの使用開始
    public void startUseParts(PartsInfo.PartsData data)
    {
        // アイテムが使用できる状態か判定
        if (!IsPartsUsable)
        {
            Debug.LogWarning("パーツの使用状態に移れませんでした。");
            return;
        }

        // 状態遷移
        _status = E_RobotStatus.UseParts;

        // TODO：アイテムの種類によって特有のアニメーションへ遷移
    }

    // パーツの効果終了
    public void endUseParts()
    {
        if (!IsUsingParts)
        {
            Debug.LogWarning("パーツの使用時以外にパーツ使用終了メソッドが呼び出されました。");
            return;
        }

        // 状態遷移（クールタイムがあれば待機状態へ移行する）
        if (cooltime <= 0) _status = E_RobotStatus.Cooldown;
        else _status = E_RobotStatus.Fly;

        // TODO：飛行orクールタイムのアニメーションに遷移する
    }

    // クールタイムの終了
    public void endCooltime()
    {
        if (!IsFlying)
        {
            Debug.LogWarning("飛行中以外にクールダウン終了メソッドが呼ばれました。");
            return;
        }

        _status = E_RobotStatus.Fly;
        cooltime = 0;

        // TODO：飛行アニメーションに遷移する
    }

    // ゲームクリア時に呼び出されるメソッド
    public void GameClear()
    {
        if (!IsFlying)
        {
            Debug.LogWarning("クリアメソッドが飛行中以外に呼び出されました。");
            return;
        }
        _status = E_RobotStatus.EndFly;

        // TODO：クリア時のアニメーションなどのロボット関係の処理
    }

    // ゲーム失敗時に呼び出されるメソッド
    public void GameOver()
    {
        if (!IsFlying)
        {
            Debug.LogWarning("ゲームオーバーメソッドが飛行中以外に呼び出されました。");
            return;
        }
        _status = E_RobotStatus.EndFly;

        // TODO：ゲーム失敗時のアニメーションなどのロボット関係の処理
    }
}
