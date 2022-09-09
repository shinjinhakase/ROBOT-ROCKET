using UnityEngine;

// 破壊可能なオブジェクトの動作を定義するComponent。
[RequireComponent(typeof(Collider2D))]
public class CrashableObject : GimickBase
{
    // ギミック攻撃タグの指定
    static private string AttackerTag = "StageAttacker";

    public bool IsAlive { get; private set; } = true;
    private Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    // ギミックをリセットするメソッド
    public override void ResetGimick() {
        gameObject.SetActive(true);
        _collider.enabled = true;
        IsAlive = true;
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

        // 自身を破棄する
        gameObject.SetActive(false);
    }
}
