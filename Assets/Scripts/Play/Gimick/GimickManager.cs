using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimickManager : SingletonMonoBehaviourInSceneBase<GimickManager>
{
    private List<GimickBase> gimickList = new List<GimickBase>();
    private List<GameObject> deleteObjects = new List<GameObject>();    // ステージリセット時に削除するオブジェクトのリスト

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
}
