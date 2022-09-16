using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  SingletonDontDestroy<U> 
            : SingletonMonoBehaviourInSceneBase<SingletonDontDestroy<U>> 
    where U : SingletonMonoBehaviourInSceneBase<SingletonDontDestroy<U>>
{
    public static new U Instance { get; private set; }

    // Start is called before the first frame update
    protected override void Awake()
    {
        if (null != Instance && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this as U;

        DontDestroyOnLoad(this);
    }
}
