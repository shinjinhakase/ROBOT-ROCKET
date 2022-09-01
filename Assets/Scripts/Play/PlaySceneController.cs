using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// �v���C�V�[���S�̂��Ǘ�����V���O���g��
public class PlaySceneController : SingletonMonoBehaviourInSceneBase<PlaySceneController>
{
    // �V�[���̏�����ʂ������񋓌^
    public E_PlayScene scene { get; private set; } = E_PlayScene.FirstCameraMove;

    [SerializeField] private CameraController cam;
    [SerializeField] private MainRobot robot;


    // �S�[����X���W
    [SerializeField] private float _goalXPoint;
    public float GoalXPoint
    {
        get { return _goalXPoint; }
        private set { _goalXPoint = value; }
    }

    [Tooltip("�J�����̈ړ��I����A�Q�[���J�n���O�ɌĂяo����郁�\�b�h")]
    [SerializeField] private UnityEvent startAnimation = new UnityEvent();

    private IEnumerator hitStopCoroutine;


    protected override void Awake()
    {
        base.Awake();

        // �J�����ړ��̏���������
        // (�K�v�ł���ΈÓ]�̉����Ȃ�)
        cam.CameraReady();
    }

    // �ŏ��̃J�����ړ����I������ۂɌĂяo����郁�\�b�h
    public void endFirstCameraMove()
    {
        if (scene == E_PlayScene.FirstCameraMove)
        {
            scene = E_PlayScene.StartAnimation;

            // �J�n�A�j���[�V�����������Ăяo��
            startAnimation.Invoke();
        }
    }
    // �Q�[�����J�n�����ۂɌĂяo����郁�\�b�h
    public void GameStart()
    {
        if (scene == E_PlayScene.StartAnimation)
        {
            scene = E_PlayScene.GamePlay;

            // TODO�F�Q�[���J�n�����i�V���h�E�ɊJ�n��`����Ȃǂ̐F�X�ȏ����j
            robot.GameStart();
        }
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

    // �V�[���̏�����ʂ������񋓌^
    public enum E_PlayScene
    {
        FirstCameraMove,    // �ŏ��̃S�[�����烍�{�b�g�܂ł̃J�����̈ړ�
        StartAnimation,     // �J�n�A�j���[�V����
        GamePlay,           // �Q�[���i�s��
        GameEnd             // �Q�[���I��
    }
}
