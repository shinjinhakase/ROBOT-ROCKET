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
    [Tooltip("SetPositionが呼び出された際の設定初期位置")]
    [SerializeField] private Vector3 _localPosition = Vector3.zero;
    [SerializeField] private float _explodeDistance = 0f;

    [Header("Rigidbody2D関連")]
    [Tooltip("Rigidbody2Dがアタッチされていた際、召喚時に加える初速設定")]
    [SerializeField] private Vector2 initVelocity = Vector2.zero;
    private bool _haveRigidbody2D;
    public bool HaveRigidbody2D { get { return _haveRigidbody2D; } private set { _haveRigidbody2D = value; } }
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

        // ステージリセット時の削除対象に登録する
        GimickManager.Instance.RegisterAsDeleteObject(gameObject);
    }
    // 召喚された際に呼び出されるメソッド（初速も設定する）
    public void Summon(PartsInfo.PartsData data, Transform robotTransform, Vector2 initVelocity)
    {
        if (IsActiveWhenSummon) gameObject.SetActive(true);
        action.Invoke(data, robotTransform);
        if (HaveRigidbody2D) _rb.velocity = initVelocity;   // Ridigbody2Dがアタッチされていれば設定された初速を加える
        afterAction.Invoke();

        // 設定がされていれば、指定秒数後に自身を破棄する
        if (IsDestroyAfterSeconds) Destroy(gameObject, _destroyDuration);

        // ステージリセット時の削除対象に登録する
        GimickManager.Instance.RegisterAsDeleteObject(gameObject);
    }

    // 位置を設定する汎用メソッド
    public void SetPosition(PartsInfo.PartsData _, Transform target)
    {
        transform.position = target.position + _localPosition;
    }
    // 親を召喚者に設定するめそっど
    public void SetParentToSummoner(PartsInfo.PartsData _, Transform summoner)
    {
        transform.SetParent(summoner);
    }
    // 位置を爆弾の位置に設定するメソッド
    public void SetPositionToBombLocate(PartsInfo.PartsData data, Transform summoner)
    {
        transform.position = summoner.position + Quaternion.Euler(0, 0, data.angle - 180) * Vector3.right * _explodeDistance;
    }

    // パーツの使用終了時間を破棄時間に設定する
    public void SetDestroyTimeToPartsDestroy(PartsInfo.PartsData data, Transform summoner)
    {
        if (data == null) return;
        var performance = PlayPartsManager.Instance.GetPerformance(data.id);
        if (performance.forceType == PartsPerformance.E_ForceType.Bomb) Destroy(gameObject);
        else if (performance.forceType == PartsPerformance.E_ForceType.NoForce) Destroy(gameObject);
        else Destroy(gameObject, performance.t);
    }

    [Serializable]
    public class SummonActionEvent : UnityEvent<PartsInfo.PartsData, Transform> { }
}
