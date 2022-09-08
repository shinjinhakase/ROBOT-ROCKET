using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトを追従する動作をするComponent
public class FollowIcon : MonoBehaviour
{
    public Transform target; // 追尾対象
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float Duration = 0.2f; // 時差

    private WaitForSeconds _waitForSeconds;

    void Awake()
    {
        _waitForSeconds = new WaitForSeconds(Duration);
    }

    void FixedUpdate()
    {
        // 取得した座標を指定秒数後にセットする
        if (target != null) StartCoroutine(UpdatePosition(target.position));
    }

    // 指定秒数後に指定の座標に合わせるメソッド
    private IEnumerator UpdatePosition(Vector3 position)
    {
        yield return _waitForSeconds;
        transform.position = position;
    }

    // 破棄メソッド
    public void DestroyMyself()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    // スプライト変更メソッド
    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
