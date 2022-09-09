using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimickManager : SingletonMonoBehaviourInSceneBase<GimickManager>
{
    private List<GimickBase> gimickList = new List<GimickBase>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ギミックを管理リストに追加する
    public void SaveGimick(GimickBase gimick)
    {
        gimickList.Add(gimick);
    }

    // 登録してある動的なステージギミックを初期化する
    public void ResetGimick()
    {
        foreach(var gimick in gimickList)
        {
            gimick.ResetGimick();
        }
    }

    // ロボットの動き始めと同時にステージギミックの処理を開始する
    public void StartGimick()
    {
        foreach(var gimick in gimickList)
        {
            gimick.OnStartRobot();
        }
    }
}
