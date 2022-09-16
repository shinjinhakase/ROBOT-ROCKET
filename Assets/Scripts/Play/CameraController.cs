using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カメラの位置を操作するComponent。
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private MainRobot robot;
    private Camera cam;
    private Transform _transform;
    private Transform robotTransform;

    private bool _initialized = false;

    // カメラの追尾フラグ
    [Header("ロボット追尾設定")]
    public bool IsFollowRobot = false;
    [Tooltip("カメラの最低位置。この位置より下にカメラが移動することはありません。")]
    [SerializeField] private float cameraUnderLimit;
    [Tooltip("ロボットのX軸の相対座標。0の場合画面中央にロボットが映る。")]
    [SerializeField] private float robotX;
    [Tooltip("ロボットのY軸の上端の相対座標。この値を超えないようにカメラが追尾する。")]
    [SerializeField] private float robotMaxY;
    [Tooltip("ロボットのY軸の下端の相対座標。この値を超えないようにカメラが追尾する。")]
    [SerializeField] private float robotMinY;

    [Header("開始時カメラ移動設定")]
    private bool IsBeginning = false;
    [Tooltip("ゴールのX軸の相対座標。開始時にゴールがこの座標に映るようにカメラを移動させる。")]
    [SerializeField] private float goalX;
    [Tooltip("ロボットのY軸の初期相対座標。開始時にY軸がこの座標に調整されます。")]
    [SerializeField] private float initY;
    [Tooltip("ステージを映すカメラの速度。この値が小さければカメラはゆっくりと動きます。")]
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
        // 最初にカメラをゴールからロボットまで移動させる。
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
        // ロボットの位置に合わせてカメラを移動させる
        else if (IsFollowRobot)
        {
            // ロボットの座標を取得
            Vector3 robotPos = robotTransform.position;

            // カメラの座標を調整する
            Vector3 cameraPos = _transform.position;
            cameraPos.x = robotPos.x - robotX;
            float robotY = robotTransform.position.y;
            if (cameraPos.y + robotMinY > robotY) cameraPos.y = robotY - robotMinY;
            else if (cameraPos.y + robotMaxY < robotY) cameraPos.y = robotY - robotMaxY;
            if (cameraPos.y < cameraUnderLimit) cameraPos.y = cameraUnderLimit;
            _transform.position = cameraPos;
        }
    }

    // カメラの最初の移動モーションの準備をする
    public void CallCameraReady()
    {
        StartCoroutine(WaitForInitialized());
    }
    private IEnumerator WaitForInitialized()
    {
        // 自身の準備が完了するまで待機する
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

        // カメラの座標をゴール位置に調整する
        Vector3 cameraPos = _transform.position;
        cameraPos.x = PlaySceneController.Instance.GoalXPoint - goalX;
        cameraPos.y = robotTransform.position.y - initY;
        _transform.position = cameraPos;

        // ロボットの初期X座標を取得する
        robotInitX = robotTransform.position.x;
    }

    // カメラをロボットの場所へセットする
    public void SetCameraToRobot()
    {
        Vector3 cameraPos = _transform.position;
        cameraPos.x = robotTransform.position.x - robotX;
        cameraPos.y = robotTransform.position.y - initY;
        _transform.position = cameraPos;
    }

    // カメラの移動が終わった際の処理
    public void endCameraBeginning()
    {
        if (!IsBeginning) return;
        IsBeginning = false;
        IsFollowRobot = true;
        PlaySceneController.Instance.endFirstCameraMove();
    }
}
