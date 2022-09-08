using System;
using UnityEngine;
using UnityEngine.Events;

// 召喚されるオブジェクトに必ず必要なComponent。召喚時に実行するメソッドをインスペクタから指定できる。
// （生成時に自身を非アクティブに設定するので、都合の良いタイミングでアクティブにする必要がある）
public class SummonableObject : MonoBehaviour
{
    [Tooltip("召喚されたときに初期設定のため呼び出される関数。引数はパーツデータ(PartsInfo.PartsData)とロボットの位置(Transform)")]
    [SerializeField] public SummonActionEvent action;
    [Tooltip("召喚時にactionが実行された後に実行される関数。引数なし。")]
    [SerializeField] public UnityEvent afterAction;

    void Awake()
    {
        // 召喚時に自身を非アクティブにしておく
        gameObject.SetActive(false);
    }

    // 召喚された際に呼び出されるメソッド
    public void Summon(PartsInfo.PartsData data, Transform robotTransform)
    {
        gameObject.SetActive(true);
        action.Invoke(data, robotTransform);
        afterAction.Invoke();
    }

    [Serializable]
    public class SummonActionEvent : UnityEvent<PartsInfo.PartsData, Transform> { }
}
