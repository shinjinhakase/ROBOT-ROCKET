using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// �v���C�V�[���S�̂��Ǘ�����V���O���g��
public class PlaySceneController : SingletonMonoBehaviourInSceneBase<PlaySceneController>
{
    // �V�[���̏�����ʂ������񋓌^
    public E_PlayScene scene { get; private set; } = E_PlayScene.FirstCameraMove;
    public int StageNum = -1;

    // �J�X�^�����j���[���J���邩�̔���
    public bool IsOpenableCustomMenu => scene == E_PlayScene.GamePlay || scene == E_PlayScene.GameEnd;

    [SerializeField] private CameraController cam;
    [SerializeField] private MainRobot robot;

    // �S�[����X���W
    [SerializeField] private float _goalXPoint;
    public float GoalXPoint { get { return _goalXPoint; } private set { _goalXPoint = value; } }
    [SerializeField] private float _score;
    public float Score {
        get { return _score; }
        set { if (_score <= _goalXPoint) _score = value; else _score = _goalXPoint; }
    }

    [Header("�C�x���g�n��")]
    [Tooltip("�J�����̈ړ��I����A�Q�[���J�n���O�ɌĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent startAnimationEvent = new UnityEvent();
    [Tooltip("�Q�[���J�n�Ɠ����ɌĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent startGameEvent = new UnityEvent();
    [Tooltip("�Q�[���I�������ہA�ǂ̂悤�ȏI�����ł����ʂ��ČĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent endGameEvent = new UnityEvent();

    private IEnumerator hitStopCoroutine;



    protected override void Awake()
    {
        base.Awake();

        // �J�����ړ��̏���������
        cam.CameraReady();

        // (�K�v�ł���΂����ňÓ]�̉����Ȃ�)
    }

    // �ŏ��̃J�����ړ����I������ۂɌĂяo����郁�\�b�h
    [ContextMenu("Scene/EndFirstCameraMove")]
    public void endFirstCameraMove()
    {
        if (scene == E_PlayScene.FirstCameraMove)
        {
            scene = E_PlayScene.StartAnimation;
            _score = 0;

            // �J�n�A�j���[�V�����������Ăяo��
            startAnimationEvent.Invoke();
        }
    }
    // �Q�[�����J�n�����ۂɌĂяo����郁�\�b�h
    [ContextMenu("Scene/GameStart")]
    public void GameStart()
    {
        if (scene == E_PlayScene.StartAnimation)
        {
            scene = E_PlayScene.GamePlay;

            // �J�n���̃C�x���g���Ăяo��
            startGameEvent.Invoke();

            // TODO�F�Q�[���J�n�����i�V���h�E�ɊJ�n��`����Ȃǂ̐F�X�ȏ����j
            robot.GameStart();
        }
    }
    // �J�X�^�����j���[���J��
    [ContextMenu("Scene/OpenCustomMenu")]
    public void OpenCustomMenu()
    {
        if (IsOpenableCustomMenu)
        {
            bool IsNeedSetResult = false;
            if (scene == E_PlayScene.GamePlay)
            {
                IsNeedSetResult = true;
            }
            scene = E_PlayScene.CustomMenu;

            if (IsNeedSetResult) endGameEvent.Invoke();

            // ��s���Ȃ�A���{�b�g��A��Ă����ăJ�X�^�����j���[���J�������Ɉڂ�
            cam.IsFollowRobot = false;
            robot.OpenCustomMenu();

            // TODO�F�J�X�^�����j���[�̃I�[�v������������
        }
    }
    // �J�X�^�����j���[������Ƃ��̏���
    [ContextMenu("Scene/CloseCustomMenu")]
    public void CloseCustomMenu()
    {
        if (scene == E_PlayScene.CustomMenu) 
        {
            scene = E_PlayScene.FirstCameraMove;

            // �X�e�[�W�����Z�b�g���A�ŏ������蒼���B
            ResetStage();

            // TODO�F�J�����̒����Ȃǂ̏���
            cam.IsFollowRobot = true;

            endFirstCameraMove();
        }
    }
    // �Q�[���N���A�������s��
    [ContextMenu("Scene/GameClear")]
    public void GameClear()
    {
        if(scene == E_PlayScene.GamePlay)
        {
            scene = E_PlayScene.GameEnd;

            endGameEvent.Invoke();

            cam.IsFollowRobot = false;
            robot.GameClear();
            // ���{�b�g�����n������A���ʕ\���Ȃǂ̏������ĂԁB
        }
    }
    // �Q�[���I�[�o�[�������s��
    [ContextMenu("Scene/GameOver")]
    public void GameOver()
    {
        if (scene == E_PlayScene.GamePlay)
        {
            scene = E_PlayScene.GameEnd;

            endGameEvent.Invoke();

            // �J�����̒ǔ���؂�A���{�b�g�̃Q�[���I�[�o�[���������s����
            cam.IsFollowRobot = false;
            robot.GameOver();
            // TODO�F���{�b�g�p�[�W�A�j���[�V�����ҋ@��ɁA���ʕ\��������Ȃǂ̏������Ă�
            // TODO�F���ʕ\����UI�ł�蒼���{�^�����������邩�A�Q�[���I�[�o�[�A�j���[�V�����̐��b��ɂ܂��J�n/�J�X�^�����肷��H
        }
    }
    // ���Z�b�g�̏������s��
    [ContextMenu("Debug/ResetStage")]
    public void ResetStage()
    {
        // �q�b�g�X�g�b�v�������~���A���Ԃ̗����߂�
        StopHitStopIfExists(true);

        // �g�p�p�[�c�󋵂ƃ��C�����{�b�g�󋵂����Z�b�g
        PlayPartsManager.Instance.ResetPartsStatus();
        robot.ResetToStart();
        // �J���������{�b�g�̏��֏�����ԂŃZ�b�g
        cam.SetCameraToRobot();

        // TODO�F�V���h�E�̏���������
        // TODO�F�X�e�[�W�̕��������i�ғ��I�u�W�F�N�g������Ȃ�j
    }


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
        StopHitStopIfExists(false);
        hitStopCoroutine = ChangeTimeScale(timeScale, time);
        StartCoroutine(hitStopCoroutine);
    }
    // ���s���Ă���q�b�g�X�g�b�v������Ȃ�A���̏����𒆒f���Ď~�߂�
    public void StopHitStopIfExists(bool ResetTimeScale)
    {
        if (hitStopCoroutine != null)
        {
            StopCoroutine(hitStopCoroutine);
            hitStopCoroutine = null;
        }
        // ResetTimeScale��true�Ȃ�A���Ԃ̑��x�����ɖ߂�
        if (ResetTimeScale) Time.timeScale = 1f;
    }

    // �w��̕b��TimeScale��ω������郁�\�b�h
    private IEnumerator ChangeTimeScale(float timeScale, float time)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }

    // �V�[���̏�����ʂ������񋓌^
    public enum E_PlayScene
    {
        FirstCameraMove,    // �ŏ��̃S�[�����烍�{�b�g�܂ł̃J�����̈ړ�
        StartAnimation,     // �J�n�A�j���[�V����
        GamePlay,           // �Q�[���i�s��
        GameEnd,            // �Q�[���I��
        CustomMenu          // �J�X�^�����j���[���J���Ă���
    }
}
