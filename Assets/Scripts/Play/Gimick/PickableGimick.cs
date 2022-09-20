using UnityEngine;
using UnityEngine.Events;

// �E����I�u�W�F�N�g�̊��N���X
[RequireComponent(typeof(Collider2D))]
public abstract class PickableGimick : GimickBase
{
    protected bool Picked;  // �E�����t���O
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

    // �E�����ۂ̏���
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out MainRobot _mainRobot) || Picked) return;
        Picked = true;

        // �E�����ۂ̃C�x���g������
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
