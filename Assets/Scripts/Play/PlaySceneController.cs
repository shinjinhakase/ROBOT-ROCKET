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
        set { if (value <= _goalXPoint) _score = value; else _score = _goalXPoint; }
    }

    [Header("�C�x���g�n��")]
    [Tooltip("�Q�[�����ɃJ�X�^����ʂֈȍ~�����ۂɃJ�X�^����ʂ��J���܂ł̒x������")]
    [SerializeField] private float OpenCustomWhenPlayDuration = 1f;
    [Tooltip("�J�����̈ړ��I����A�Q�[���J�n���O�ɌĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent startAnimationEvent = new UnityEvent();
    [Tooltip("�Q�[���J�n�Ɠ����ɌĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent startGameEvent = new UnityEvent();
    [Tooltip("�Q�[���J�n�ネ�{�b�g�������n�߂��ۂɌĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent startRobotMove = new UnityEvent();
    [Tooltip("�Q�[���I�������ہA�ǂ̂悤�ȏI�����ł����ʂ��ČĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent endGameEvent = new UnityEvent();
    [Tooltip("�Q�[���I�[�o�[�ƂȂ����ۂɌĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent gameOverEvent = new UnityEvent();
    [Tooltip("�Q�[���N���A�����ۂɌĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent gameClearEvent = new UnityEvent();
    [Tooltip("�J�X�^�����j���[���o���ۂɌĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent OpenCustomMenuEvent = new UnityEvent();
    [Tooltip("���݂̃��v���C���m�F���鏈���Ɉȍ~�����ۂɁA�X�e�[�W���Z�b�g���O�ɌĂяo����鏈��")]
    [SerializeField] private UnityEvent CheckReplayEvent = new UnityEvent();

    private IEnumerator hitStopCoroutine;

    private Stage _currentStage = new Stage();
    public Stage CurrentStage { get { return _currentStage; } private set { _currentStage = value; } }


    protected override void Awake()
    {
        base.Awake();

        // �J�����ړ��̏���������
        cam.CameraReady();

        // (�K�v�ł���΂����ňÓ]�̉����Ȃ�)
        InitStageInfo();
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
    // ���{�b�g�������n�߂��ۂɌĂяo����郁�\�b�h
    public void RobotStartMove()
    {
        if (scene == E_PlayScene.GamePlay)
        {
            startRobotMove.Invoke();
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

            // ��s���Ȃ�A���{�b�g��A��Ă����ăJ�X�^�����j���[���J�������Ɉڂ�
            cam.IsFollowRobot = false;
            robot.OpenCustomMenu();

            if (IsNeedSetResult) endGameEvent.Invoke();

            // �p�[�c�̏�Ԃ��J�X�^�����ɖ߂�
            PartsInfo.Instance.Reset();

            // �J�X�^�����j���[�̃I�[�v������
            if (IsNeedSetResult && robot._status.IsFlying) {
                CallMethodAfterDuration(OpenCustomMenuEvent.Invoke, OpenCustomWhenPlayDuration);
            }
            else
            {
                OpenCustomMenuEvent.Invoke();
            }
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

            cam.IsFollowRobot = false;
            robot.GameClear();

            endGameEvent.Invoke();

            gameClearEvent.Invoke();
        }
    }
    // �Q�[���I�[�o�[�������s��
    [ContextMenu("Scene/GameOver")]
    public void GameOver()
    {
        if (scene == E_PlayScene.GamePlay)
        {
            scene = E_PlayScene.GameEnd;

            // �J�����̒ǔ���؂�A���{�b�g�̃Q�[���I�[�o�[���������s����
            cam.IsFollowRobot = false;
            robot.GameOver();

            endGameEvent.Invoke();
            
            // ���{�b�g�p�[�W�A�j���[�V�����ҋ@��ɁA���ʕ\��������Ȃǂ̏������Ă�
            gameOverEvent.Invoke();
        }
    }
    // ���݂̃v���C�����v���C�Ŋm�F����
    [ContextMenu("Scene/CheckReplay")]
    public void CheckReplay()
    {
        if (scene == E_PlayScene.GameEnd)
        {
            scene = E_PlayScene.FirstCameraMove;

            // ���{�b�g�����v���C���[�h�ɐݒ肷��
            CheckReplayEvent.Invoke();
            robot.SetReplayMode();

            // �X�e�[�W�����Z�b�g���A�ŏ������蒼���B
            ResetStage();

            // TODO�F�J�����̒����Ȃǂ̏���
            cam.IsFollowRobot = true;

            Invoke("endFirstCameraMove", 0.1f);
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
    // �w��̃��\�b�h���w�莞�Ԍ�ɌĂяo�����\�b�h
    private void CallMethodAfterDuration(Action action, float time)
    {
        StartCoroutine(CallMethodAfterDurationEnumerator(action, time));
    }
    private IEnumerator CallMethodAfterDurationEnumerator(Action action, float time)
    {
        if (time > 0) yield return new WaitForSeconds(time);
        action();
        yield break;
    }
    // �X�e�[�W�����󂯎�郁�\�b�h
    private void InitStageInfo()
    {
        StageSelectGlobal _stageSelectGlobal = StageSelectGlobal.Instance;
        if (_stageSelectGlobal)
        {
            CurrentStage = _stageSelectGlobal.Stage;
            StageNum = CurrentStage.StageNum;
            /* �S�[�����W�͂ǂ����邩 */
        }
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
