using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ����I�ɃA�N�V�������N�����M�~�b�N�̊��Component�B
public class IntervalActionGimick : GimickBase
{
    // �ҋ@�b��
    [SerializeField] protected float _initWait; // �J�n����̑ҋ@�b��
    [SerializeField] protected float _interval; // �C���^�[�o���i�b�j

    // ����I�ɌĂяo�����A�N�V�����C�x���g
    [SerializeField] private UnityEvent _actionEvent = new UnityEvent();

    private IEnumerator intervalCaller;

    // ���{�b�g�������n�߂��ۂɓ����𓯊����郁�\�b�h
    public override void OnStartRobot() {
        if (intervalCaller != null)
        {
            StopCoroutine(intervalCaller);
            intervalCaller = null;
        }
        intervalCaller = IntervalActionCall();
        StartCoroutine(intervalCaller);
    }

    // �M�~�b�N�����Z�b�g���郁�\�b�h
    public override void ResetGimick() {
        if (intervalCaller != null) {
            StopCoroutine(intervalCaller);
            intervalCaller = null;
        }
    }

    // ����I�ɌĂяo�����A�N�V�������\�b�h
    protected virtual void Action() {
        _actionEvent.Invoke();
    }

    // ����I�ɃA�N�V�������Ăяo�����\�b�h
    private IEnumerator IntervalActionCall()
    {
        if (_initWait > 0) yield return new WaitForSeconds(_initWait);

        WaitForSeconds waitForSeconds = new WaitForSeconds(_interval);
        while (true)
        {
            Action();
            yield return waitForSeconds;
        }
    }
}
