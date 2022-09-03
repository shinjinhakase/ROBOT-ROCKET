using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayInputManager : SingletonMonoBehaviourInSceneBase<ReplayInputManager>
{
    // 現在のプレイのリプレイデータ
    [SerializeField]
    private ReplayData _data = new ReplayData();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
