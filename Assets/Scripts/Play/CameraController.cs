using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �J�����̈ʒu�𑀍삷��Component�B
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private MainRobot robot;
    private Camera cam;
    private Transform _transform;
    private Transform robotTransform;

    private bool _initialized = false;

    // �J�����̒ǔ��t���O
    [Header("���{�b�g�ǔ��ݒ�")]
    public bool IsFollowRobot = false;
    [Tooltip("�J�����̍Œ�ʒu�B���̈ʒu��艺�ɃJ�������ړ����邱�Ƃ͂���܂���B")]
    [SerializeField] private float cameraUnderLimit;
    [Tooltip("���{�b�g��X���̑��΍��W�B0�̏ꍇ��ʒ����Ƀ��{�b�g���f��B")]
    [SerializeField] private float robotX;
    [Tooltip("���{�b�g��Y���̏�[�̑��΍��W�B���̒l�𒴂��Ȃ��悤�ɃJ�������ǔ�����B")]
    [SerializeField] private float robotMaxY;
    [Tooltip("���{�b�g��Y���̉��[�̑��΍��W�B���̒l�𒴂��Ȃ��悤�ɃJ�������ǔ�����B")]
    [SerializeField] private float robotMinY;

    [Header("�J�n���J�����ړ��ݒ�")]
    private bool IsBeginning = false;
    [Tooltip("�S�[����X���̑��΍��W�B�J�n���ɃS�[�������̍��W�ɉf��悤�ɃJ�������ړ�������B")]
    [SerializeField] private float goalX;
    [Tooltip("���{�b�g��Y���̏������΍��W�B�J�n����Y�������̍��W�ɒ�������܂��B")]
    [SerializeField] private float initY;
    [Tooltip("�X�e�[�W���f���J�����̑��x�B���̒l����������΃J�����͂������Ɠ����܂��B")]
    [SerializeField] private float cameraVelocity;
    private float robotInitX;

    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();
        _transform = cam.transform;
        robotTransform = robot.transform;
        _initialized = true;
    }

    private void LateUpdate()
    {
        // �ŏ��ɃJ�������S�[�����烍�{�b�g�܂ňړ�������B
        if (IsBeginning)
        {
            if (_transform.position.x < robotInitX - robotX)
            {
                endCameraBeginning();
            }
            else
            {
                _transform.position += Vector3.left * cameraVelocity;
            }
        }
        // ���{�b�g�̈ʒu�ɍ��킹�ăJ�������ړ�������
        else if (IsFollowRobot)
        {
            // ���{�b�g�̍��W���擾
            Vector3 robotPos = robotTransform.position;

            // �J�����̍��W�𒲐�����
            Vector3 cameraPos = _transform.position;
            cameraPos.x = robotPos.x - robotX;
            float robotY = robotTransform.position.y;
            if (cameraPos.y + robotMinY > robotY) cameraPos.y = robotY - robotMinY;
            else if (cameraPos.y + robotMaxY < robotY) cameraPos.y = robotY - robotMaxY;
            if (cameraPos.y < cameraUnderLimit) cameraPos.y = cameraUnderLimit;
            _transform.position = cameraPos;
        }
    }

    // �J�����̍ŏ��̈ړ����[�V�����̏���������
    public void CallCameraReady()
    {
        StartCoroutine(WaitForInitialized());
    }
    private IEnumerator WaitForInitialized()
    {
        // ���g�̏�������������܂őҋ@����
        while (true)
        {
            if (_initialized)
            {
                CameraReady();
                yield break;
            }
            yield return null;
        }
    }
    private void CameraReady()
    {
        IsBeginning = true;

        // �J�����̍��W���S�[���ʒu�ɒ�������
        Vector3 cameraPos = _transform.position;
        cameraPos.x = PlaySceneController.Instance.GoalXPoint - goalX;
        cameraPos.y = robotTransform.position.y - initY;
        _transform.position = cameraPos;

        // ���{�b�g�̏���X���W���擾����
        robotInitX = robotTransform.position.x;
    }

    // �J���������{�b�g�̏ꏊ�փZ�b�g����
    public void SetCameraToRobot()
    {
        Vector3 cameraPos = _transform.position;
        cameraPos.x = robotTransform.position.x - robotX;
        cameraPos.y = robotTransform.position.y - initY;
        _transform.position = cameraPos;
    }

    // �J�����̈ړ����I������ۂ̏���
    public void endCameraBeginning()
    {
        if (!IsBeginning) return;
        IsBeginning = false;
        IsFollowRobot = true;
        PlaySceneController.Instance.endFirstCameraMove();
    }
}
