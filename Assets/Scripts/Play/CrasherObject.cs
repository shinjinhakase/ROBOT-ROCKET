using System.Collections;
using UnityEngine;

// 破壊可能オブジェクト(CrashableObject)を破壊可能なオブジェクトを定義するComponent。
public class CrasherObject : MonoBehaviour
{
    [Tooltip("攻撃開始の当たり判定を担うCollider2Dをセットする")]
    [SerializeField] private Collider2D CollisionDetectCollider;

    [Tooltip("攻撃の当たり判定を担うCollider2Dをセットする")]
    [SerializeField] private Collider2D AttackRangeCollider;

    [Tooltip("攻撃の当たり判定が出続ける秒数")]
    [SerializeField] private float AttackTime = 1f;

    [Tooltip("攻撃が終了してからオブジェクトが破棄されるまでのタイムラグ")]
    [SerializeField] private float DestroyDuration = 0f;

    public bool IsStartAttack { get; private set; } = false;

    // Start is called before the first frame update
    private void Awake()
    {
        // 当たり判定の有効化を確認しておく
        CollisionDetectCollider.enabled = true;
        AttackRangeCollider.enabled = false;
    }

    [ContextMenu("Debug/StartAttack")]
    private void StartAttackForDebug()
    {
        StartAttack(CollisionDetectCollider);
    }

    // 攻撃開始時に当たり判定Colliderから呼び出される、衝突メソッド
    public void StartAttack(Collider2D other)
    {
        if (IsStartAttack) return;
        IsStartAttack = true;

        // 衝突判定をオフにし、攻撃の当たり判定をオンにする
        CollisionDetectCollider.enabled = false;
        AttackRangeCollider.enabled = true;

        // 衝突以降の処理を始める
        StartCoroutine(ControlAttack());
    }

    // 攻撃を開始してからの処理管理メソッド
    private IEnumerator ControlAttack()
    {
        yield return new WaitForSeconds(AttackTime);
        AttackRangeCollider.enabled = false;
        Destroy(gameObject, DestroyDuration);
    }
}
