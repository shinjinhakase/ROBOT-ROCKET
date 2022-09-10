using System;
using UnityEngine;
using UnityEngine.Events;

// 召喚されるオブジェクトに必ず必要なComponent。召喚時に実行するメソッドをインスペクタから指定できる。
// （生成時に自身を非アクティブに設定するので、都合の良いタイミングでアクティブにする必要がある）
public class SummonableObject : MonoBehaviour
{
    [Header("イベント関連")]
    [Tooltip("召喚されたときに初期設定のため呼び出される関数。引数はパーツデータ(PartsInfo.PartsData)と召喚者の位置(Transform)")]
    [SerializeField] public SummonActionEvent action;
    [Tooltip("召喚時にactionが実行された後に実行される関数。引数なし。")]
    [SerializeField] public UnityEvent afterAction;

    [Header("召喚時設定")]
    [Tooltip("trueにすると、召喚時に自身をActiveにする")]
    [SerializeField] private bool IsActiveWhenSummon = true;
    [Tooltip("trueにすると、召還後に指定秒数経つと自動で消滅する")]
    [SerializeField] private bool IsDestroyAfterSeconds = false;
    [Tooltip("IsDestroyAfterSecondsがtrueの際の、消滅秒数設定")]
    [SerializeField] private float _destroyDuration = 0f;

    [Header("Rigidbody2D関連")]
    [Tooltip("Rigidbody2Dがアタッチされていた際、召喚時に加える初速設定")]
    [SerializeField] private Vector2 initVelocity = Vector2.zero;
    private bool HaveRigidbody2D;
    private Rigidbody2D _rb;

    void Awake()
    {
        // 召喚時に自身を非アクティブにしておく
        gameObject.SetActive(false);

        HaveRigidbody2D = TryGetComponent(out _rb);
    }

    // 召喚された際に呼び出されるメソッド
    public void Summon(PartsInfo.PartsData data, Transform robotTransform)
    {
        if (IsActiveWhenSummon) gameObject.SetActive(true);
        action.Invoke(data, robotTransform);
        if (HaveRigidbody2D) _rb.velocity = initVelocity;   // Ridigbody2Dがアタッチされていれば設定された初速を加える
        afterAction.Invoke();

        // 設定がされていれば、指定秒数後に自身を破棄する
        if (IsDestroyAfterSeconds) Destroy(gameObject, _destroyDuration);
    }

    [Serializable]
    public class SummonActionEvent : UnityEvent<PartsInfo.PartsData, Transform> { }
}
