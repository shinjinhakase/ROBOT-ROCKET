using System;
using UnityEngine;
using UnityEngine.Events;

// 召喚されるオブジェクトに必ず必要なComponent。召喚時に実行するメソッドをインスペクタから指定できる。
// （生成時に自身を非アクティブに設定するので、都合の良いタイミングでアクティブにする必要がある）
public class SummonableObject : MonoBehaviour
{
    [Tooltip("召喚されたときに呼び出される関数。引数はパーツデータ(PartsInfo.PartsData)とロボットの位置(Transform)")]
    [SerializeField] public SummonActionEvent action;

    void Awake()
    {
        // 召喚時に自身を非アクティブにしておく
        gameObject.SetActive(false);
    }

    // 召喚された際に呼び出されるメソッド
    public void Summon(PartsInfo.PartsData data, Transform robotTransform)
    {
        action.Invoke(data, robotTransform);
    }

    [Serializable]
    public class SummonActionEvent : UnityEvent<PartsInfo.PartsData, Transform> { }
}
