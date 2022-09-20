using UnityEngine;
using UnityEngine.Events;

// 拾えるオブジェクトの基底クラス
[RequireComponent(typeof(Collider2D))]
public abstract class PickableGimick : GimickBase
{
    protected bool Picked;  // 拾ったフラグ
    protected Collider2D _collider;

    [SerializeField] protected UnityEvent _pickedEvent = new UnityEvent();
    [SerializeField] protected UnityEvent _pickedEventOnlyPlay = new UnityEvent();

    public virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public override void ResetGimick()
    {
        Picked = false;
        gameObject.SetActive(true);
    }

    // 拾った際の処理
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out MainRobot _mainRobot) || Picked) return;
        Picked = true;

        // 拾った際のイベントを処理
        if (!PlaySceneController.Instance.IsReplayMode)
        {
            PickedActionOnlyPlay(_mainRobot);
            _pickedEventOnlyPlay.Invoke();
        }
        PickedAction(_mainRobot);
        _pickedEvent.Invoke();

        gameObject.SetActive(false);
    }

    protected virtual void PickedActionOnlyPlay(MainRobot mainRobot) { }
    protected virtual void PickedAction(MainRobot mainRobot) { }
}
