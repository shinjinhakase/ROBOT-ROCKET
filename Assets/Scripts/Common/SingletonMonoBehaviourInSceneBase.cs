using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// シーンに1つしか存在しないオブジェクトのシングルトン
// 継承すると、クラス名.Instanceでインスタンスにアクセス可能になる
public class SingletonMonoBehaviourInSceneBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if(null != Instance && Instance != this)
        {
            // シーンに2つ以上インスタンスが存在する場合は、エラーを出す
            throw new Exception("シーン内に他のインスタンスが存在しています！");
        }

        Instance = this as T;
    }

    // Awake()でInstanceがセットされた後に指定のメソッドを実行する
    public static IEnumerator ActionAfterSetInstance(UnityAction action)
    {
        while (true)
        {
            if (Instance != null)
            {
                action();
                yield break;
            }
            yield return null;
        }
    }
}
