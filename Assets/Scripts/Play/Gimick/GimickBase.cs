using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 管理側から管理する必要のある動的ステージギミックの抽象クラス
public abstract class GimickBase : MonoBehaviour
{
    private bool IsMemoried = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        StartCoroutine(RequestMemoryToManager());
    }

    // シーンが開始した際に呼ばれるメソッド
    public virtual void OnSceneStart() { }

    // ロボットが動き始めた際に動きを同期するメソッド
    public virtual void OnStartRobot() { }

    // ギミックをリセットするメソッド
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
