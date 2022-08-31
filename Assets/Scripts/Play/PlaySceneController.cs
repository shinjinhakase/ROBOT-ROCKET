using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �v���C�V�[���S�̂��Ǘ�����V���O���g��
public class PlaySceneController : SingletonMonoBehaviourInSceneBase<PlaySceneController>
{
    IEnumerator hitStopCoroutine;


    // �ꎞ��~����`�Ńq�b�g�X�g�b�v����������
    public void RequestHitStopByStop(float time)
    {
        if (time <= 0) return;
        SetHitStop(0, time);
    }

    // �X���[�ɂȂ�`�Ńq�b�g�X�g�b�v����������
    public void RequestHitStopBySlow(float timeScale, float time)
    {
        if (time <= 0 || timeScale < 0) return;
        SetHitStop(timeScale, time);
    }


    // �V���Ɏw��̕b���������Ԍo�ߑ��x��ύX����
    private void SetHitStop(float timeScale, float time)
    {
        // ���s���Ă����q�b�g�X�g�b�v�����𒆒f���A�V���Ƀq�b�g�X�g�b�v�������n�߂�
        if(hitStopCoroutine != null)
        {
            StopCoroutine(hitStopCoroutine);
            hitStopCoroutine = null;
        }
        hitStopCoroutine = ChangeTimeScale(timeScale, time);
        StartCoroutine(hitStopCoroutine);
    }

    // �w��̕b��TimeScale��ω������郁�\�b�h
    private IEnumerator ChangeTimeScale(float timeScale, float time)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }
}
