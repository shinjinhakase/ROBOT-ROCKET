using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �X�e�[�W�ԍ��̏����ꂽ�Ŕ̐���Component
public class StageNumBoard : NumbersManager
{
    private bool IsStartMotion = false;
    private int finalFrame;
    private int frameCnt = 0;
    [SerializeField] private float waitDuration;// �ҋ@����
    [SerializeField] private float time;        // �|���鎞��
    [SerializeField] private float distance;    // �ړ����鋗��
    private Vector3 _homePosition;

    [Tooltip("�ړ����[�V�������I������ۂ�PlaySceneController�̎��̏������Ăяo��")]
    [SerializeField] private bool IsCallControllerWhenEndMotion = true;

    void Start()
    {
        StartCoroutine(SetStageNumCoroutine());
        finalFrame = Mathf.RoundToInt(Mathf.Abs(time) / Time.fixedDeltaTime);
        _homePosition = transform.position;
        _homePosition = new Vector3(_homePosition.x, _homePosition.y, _homePosition.z);
    }

    private void FixedUpdate()
    {
        if (IsStartMotion)
        {
            // �ʒu���X�V����
            transform.position = _homePosition + Vector3.up * distance / finalFrame * frameCnt;
            frameCnt++;
            if (frameCnt >= finalFrame)
            {
                IsStartMotion = false;
                gameObject.SetActive(false);
                if (IsCallControllerWhenEndMotion) PlaySceneController.Instance.GameStart();
            }
        }
    }

    // �J�n���̃��[�V�������J�n����
    public void StartMotion()
    {
        frameCnt = 0;
        transform.position = _homePosition;
        gameObject.SetActive(true);
        Invoke("OnStartFlag", waitDuration);
    }
    private void OnStartFlag()
    {
        IsStartMotion = true;
    }

    // �X�e�[�W�ԍ����Z�b�g����
    private IEnumerator SetStageNumCoroutine()
    {
        while (true)
        {
            PlaySceneController _playSceneController = PlaySceneController.Instance;
            if(_playSceneController)
            {
                UpdateNum(_playSceneController.StageNum);
                yield break;
            }
            yield return null;
        }
    }
}
