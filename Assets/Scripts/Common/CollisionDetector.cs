using System;
using UnityEngine;
using UnityEngine.Events;

// �����蔻����Ǘ�����Component�B���\�b�h�̌Ăяo�����y�ɊǗ��ł���B
public class CollisionDetector : MonoBehaviour
{
    [Tooltip("�Փ˂����ۂɌĂяo����郁�\�b�h")]
    [SerializeField] private CollisionEvent OnTriggerEnter = new CollisionEvent();
    [Tooltip("�d�Ȃ��Ă���Ԃ����ƌĂяo����郁�\�b�h")]
    [SerializeField] private CollisionEvent OnTriggerStay = new CollisionEvent();
    [Tooltip("���ꂽ�یĂяo����郁�\�b�h")]
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
