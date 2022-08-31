using System.Collections;
using UnityEngine;

// 破壊可能オブジェクト(CrashableObject)を破壊可能なオブジェクトを定義するComponent。
public class CrasherObject : MonoBehaviour
{
    private enum E_DisableSetting
    {
        NoSetting,
        DisableWhenStartAttack,
        DisableWhenEndAttack
    }

    [Tooltip("攻撃開始の当たり判定を担うCollider2Dをセットする")]
    [SerializeField] private Collider2D CollisionDetectCollider;

    [Tooltip("攻撃の当たり判定を担うCollider2Dをセットする")]
    [SerializeField] private Collider2D AttackRangeCollider;

    [Tooltip("攻撃の当たり判定が出続ける秒数")]
    [SerializeField] private float AttackTime = 1f;

    [Tooltip("攻撃が終了してからオブジェクトが破棄されるまでのタイムラグ")]
    [SerializeField] private float DestroyDuration = 0f;

    // 物理・判定動作に関わる設定
    [Header("詳細設定")]
    [Tooltip("もしRigidbody2Dがアタッチされているなら、指定のタイミングで移動・回転を固定します")]
    [SerializeField] private E_DisableSetting DisableRidigbody2DSetting = E_DisableSetting.NoSetting;
    [Tooltip("もし判定に用いていないCollider2Dがアタッチされているなら、指定のタイミングで無効化します")]
    [SerializeField] private E_DisableSetting DisableCollider2DSetting = E_DisableSetting.NoSetting;


    public bool IsStartAttack { get; private set; } = false;

    private IEnumerator timerCoroutine;
    private Rigidbody2D rb = null;
    private Collider2D _collider = null;

    // Start is called before the first frame update
    private void Awake()
    {
        // 当たり判定の有効化を確認しておく
        CollisionDetectCollider.enabled = true;
        AttackRangeCollider.enabled = false;

        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    [ContextMenu("Debug/StartAttack")]
    private void StartAttackForDebug()
    {
        StartAttack(CollisionDetectCollider);
    }

    // 攻撃タイマーをスタートさせる
    public void StartTimer(float time)
    {
        if (timerCoroutine != null || IsStartAttack) return;
        timerCoroutine = SetTimerCoroutine(time);
        StartCoroutine(timerCoroutine);
    }

    // 攻撃開始時に当たり判定Colliderから呼び出される、衝突メソッド
    public void StartAttack(Collider2D other)
    {
        if (IsStartAttack) return;
        IsStartAttack = true;

        // タイマーが設定されていた場合は切る
        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        // Ridigbody2Dの移動を設定次第で無効化する
        if(rb != null && DisableRidigbody2DSetting == E_DisableSetting.DisableWhenStartAttack)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        // Collider2Dを設定次第で無効化する
        if (_collider != null && DisableCollider2DSetting == E_DisableSetting.DisableWhenStartAttack)
        {
            _collider.enabled = false;
        }

        // 衝突判定をオフにし、攻撃の当たり判定をオンにする
        CollisionDetectCollider.enabled = false;
        AttackRangeCollider.enabled = true;

        // 衝突以降の処理を始める
        StartCoroutine(ControlAttack());
    }

    // 指定秒数後に攻撃開始メソッドを呼び出すメソッド
    private IEnumerator SetTimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        StartAttack(null);
    }

    // 攻撃を開始してからの処理管理メソッド
    private IEnumerator ControlAttack()
    {
        yield return new WaitForSeconds(AttackTime);
        AttackRangeCollider.enabled = false;
        // Ridigbody2Dの移動を設定次第で無効化する
        if (rb != null && DisableRidigbody2DSetting == E_DisableSetting.DisableWhenEndAttack)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        // Collider2Dを設定次第で無効化する
        if (_collider != null && DisableCollider2DSetting == E_DisableSetting.DisableWhenEndAttack)
        {
            _collider.enabled = false;
        }
        Destroy(gameObject, DestroyDuration);
    }
}
