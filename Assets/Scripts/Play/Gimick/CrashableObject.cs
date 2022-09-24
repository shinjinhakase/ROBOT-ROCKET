using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 破壊可能なオブジェクトの動作を定義するComponent。
[RequireComponent(typeof(Collider2D))]
public class CrashableObject : GimickBase
{
    // ギミック攻撃タグの指定
    static private string AttackerTag = "StageAttacker";

    public bool IsAlive { get; private set; } = true;
    private Collider2D _collider;

    [SerializeField] private float _destroyDuration = 0.1f;
    [Tooltip("破壊を待つ時間に表示されるスプライト。設定されていなければ、スプライトを変更しない。")]
    [SerializeField] private Sprite _destroyAnimationSprite;
    [SerializeField] private PlaySceneController.E_HitStopType _hitEffectType = PlaySceneController.E_HitStopType.None;
    [SerializeField] private float _hitEffectTimeScale;
    [SerializeField] private float _hitEffectTime;
    [SerializeField] private UnityEvent _crashEvent = new UnityEvent();
    [SerializeField] private UnityEvent _resetEvent = new UnityEvent();

    // パージスプライトの設定
    [Header("パージ設定")]
    [SerializeField] private PurgeManager _purgeManager;
    [SerializeField] private List<Sprite> _purgeSprites = new List<Sprite>();

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    // ギミックをリセットするメソッド
    public override void ResetGimick() {
        gameObject.SetActive(true);
        _collider.enabled = true;
        IsAlive = true;
        _resetEvent.Invoke();
    }

    // 何かと衝突した際、呼ばれるメソッド
    public void HitAttackCollider(Collider2D collision)
    {
        // 衝突したのが攻撃判定だったら、破壊メソッドを呼ぶ。
        if (!collision.CompareTag(AttackerTag)) return;
        Crash();
    }

    // 破壊時に呼び出されるメソッド
    [ContextMenu("Debug/Crash")]
    private void Crash()
    {
        if (!IsAlive) return;
        IsAlive = false;

        // 当たり判定を無効化
        _collider.enabled = false;
        if (gameObject.layer == LayerMask.NameToLayer("StageWall"))
        {
            // 地面の当たり判定を無効化する際はロボットの接地判定をリセットする
            GimickManager.Instance.ResetOnGround();
        }

        // 自身を破棄する
        _crashEvent.Invoke();
        if (_destroyAnimationSprite != null && TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = _destroyAnimationSprite;
        }
        Invoke("DestroyAction", _destroyDuration);
    }

    // 破壊時の処理
    private void DestroyAction()
    {
        // パージパーツの設定を行う
        if (_purgeManager)
        {
            _purgeManager.AddPartsBySprite(_purgeSprites);
        }
        gameObject.SetActive(false);
    }

    // 指定された設定でヒットストップを準備する
    public void RequestHitStop()
    {
        if (_hitEffectType == PlaySceneController.E_HitStopType.Slow)
        {
            PlaySceneController.Instance.RequestHitStopBySlow(_hitEffectTimeScale, _hitEffectTime);
        }
        else if (_hitEffectType == PlaySceneController.E_HitStopType.Stop)
        {
            PlaySceneController.Instance.RequestHitStopByStop(_hitEffectTime);
        }
    }
}
