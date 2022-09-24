using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimickManager : SingletonMonoBehaviourInSceneBase<GimickManager>
{
    private List<GimickBase> gimickList = new List<GimickBase>();
    private List<GameObject> deleteObjects = new List<GameObject>();    // ステージリセット時に削除するオブジェクトのリスト

    [SerializeField] private List<SoundBeep> soundBeeps = new List<SoundBeep>();

    [SerializeField] private MainRobot _mainRobot;

    // ギミックを管理リストに追加する
    public void SaveGimick(GimickBase gimick)
    {
        gimickList.Add(gimick);
    }

    // 登録してある動的なステージギミックを初期化する
    public void ResetGimick()
    {
        // ギミックをリセット
        foreach(var gimick in gimickList)
        {
            gimick.ResetGimick();
        }
        
        // 削除対象オブジェクトを削除
        foreach(var deleteObject in deleteObjects)
        {
            if (deleteObject) Destroy(deleteObject);
        }
        deleteObjects.Clear();
    }

    // ロボットの動き始めと同時にステージギミックの処理を開始する
    public void StartGimick()
    {
        foreach(var gimick in gimickList)
        {
            gimick.OnStartRobot();
        }
    }


    // リセット時の削除対象オブジェクトに登録
    public void RegisterAsDeleteObject(GameObject target)
    {
        deleteObjects.Add(target);
    }

    // ブロック破壊の音を鳴らす
    public void BeepSEByIndex(int index)
    {
        if (index < 0 || index >= gimickList.Count) {
            Debug.LogWarning("効果音のインデックス指定が不正です。");
            return;
        }
        soundBeeps[index].Beep();
    }

    // 地面の当たり判定がDestroy等で消えた際はOnTriggerExitが反応しないので、AnimatorのOnGround（接地判定）をリセットする。
    public void ResetOnGround()
    {
        _mainRobot._status.SetOnGround(false);
        ShadowManager.Instance.ResetOnGround();
    }
}
