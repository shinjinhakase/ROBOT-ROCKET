using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ǘ�������Ǘ�����K�v�̂��铮�I�X�e�[�W�M�~�b�N�̒��ۃN���X
public abstract class GimickBase : MonoBehaviour
{
    private bool IsMemoried = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        StartCoroutine(RequestMemoryToManager());
    }

    // �V�[�����J�n�����ۂɌĂ΂�郁�\�b�h
    public virtual void OnSceneStart() { }

    // ���{�b�g�������n�߂��ۂɓ����𓯊����郁�\�b�h
    public virtual void OnStartRobot() { }

    // �M�~�b�N�����Z�b�g���郁�\�b�h
    public virtual void ResetGimick() { }

    private IEnumerator RequestMemoryToManager()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        while (!IsMemoried)
        {
            GimickManager _gimickManager = GimickManager.Instance;
            if (_gimickManager != null)
            {
                IsMemoried = true;
                _gimickManager.SaveGimick(this);
                OnSceneStart();
                yield break;
            }
            yield return waitForEndOfFrame;
        }
    }
}
