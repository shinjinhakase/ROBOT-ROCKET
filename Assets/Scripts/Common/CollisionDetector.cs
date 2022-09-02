using System;
using UnityEngine;
using UnityEngine.Events;

// 当たり判定を管理するComponent。メソッドの呼び出しが楽に管理できる。
public class CollisionDetector : MonoBehaviour
{
    [Tooltip("衝突した際に呼び出されるメソッド")]
    [SerializeField] private CollisionEvent OnTriggerEnter = new CollisionEvent();
    [Tooltip("重なっている間ずっと呼び出されるメソッド")]
    [SerializeField] private CollisionEvent OnTriggerStay = new CollisionEvent();
    [Tooltip("離れた際呼び出されるメソッド")]
    [SerializeField] private CollisionEvent OnTriggerExit = new CollisionEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerStay.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit.Invoke(collision);
    }

    [Serializable]
    public class CollisionEvent : UnityEvent<Collider2D> { }
}
