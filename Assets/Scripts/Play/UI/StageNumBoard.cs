using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステージ番号の書かれた看板の制御Component
public class StageNumBoard : NumbersManager
{
    private bool IsStartMotion = false;
    private int finalFrame;
    private int frameCnt = 0;
    [SerializeField] private float waitDuration;// 待機時間
    [SerializeField] private float time;        // 掛ける時間
    [SerializeField] private float distance;    // 移動する距離
    private Vector3 _homePosition;

    [Tooltip("移動モーションが終わった際にPlaySceneControllerの次の処理を呼び出す")]
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
            // 位置を更新する
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

    // 開始時のモーションを開始する
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

    // ステージ番号をセットする
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
