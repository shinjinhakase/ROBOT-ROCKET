using System.Collections.Generic;
using UnityEngine;

// ロボットパーツ使用後ののパージや、ゲームオーバー時のロボットのパージなどのパージオブジェクトを管理するComponent。
public class PurgeManager : MonoBehaviour
{
    [Tooltip("パージするパーツの基礎となるプレファブ。スプライトのみを指定された場合、この見た目だけが変更される。")]
    [SerializeField] private Rigidbody2D purgePartsPrefab;

    [Header("パージ時の初速設定")]
    [Tooltip("消滅するまでの時間")]
    [SerializeField] private float DestroyDuration = 2f;
    [Tooltip("最小の初速度")]
    [SerializeField] private float initMaxVelocity = 4f;
    [Tooltip("最大の初速度")]
    [SerializeField] private float initMinVelocity = 2f;
    [Tooltip("投げられる方向の角度幅")]
    [SerializeField] private float initAngleRange = 120f;
    [Tooltip("回転の角速度")]
    [SerializeField] private float initAngleVelocity = 0f;

    // パージするパーツリストに指定のデータをプレファブを指定して追加する
    public void AddPartsByPrefab(List<Rigidbody2D> purgePartsGroup)
    {
        if (purgePartsGroup == null || purgePartsGroup.Count == 0) return;
        Vector3 position = transform.position;
        foreach (var prefab in purgePartsGroup)
        {
            // オブジェクトを生成し、初速度などを設定する
            var purgeParts = Instantiate(prefab, position, Quaternion.identity);
            // 初期設定を済ませる
            SetFirstSettings(ref purgeParts);
        }
    }
    public void AddPartsByPrefab(Rigidbody2D purgePartsPrefab)
    {
        if (purgePartsPrefab == null ) return;
        Vector3 position = transform.position;
        // オブジェクトを生成し、初速度などを設定する
        var purgeParts = Instantiate(purgePartsPrefab, position, Quaternion.identity);
        // 初期設定を済ませる
        SetFirstSettings(ref purgeParts);
    }

    // パージするパーツリストに指定のデータをスプライトから構築して追加する
    public void AddPartsBySprite(List<Sprite> purgePartsSprites)
    {
        if (!(purgePartsSprites?.Count > 0)) return;
        Vector3 position = transform.position;
        foreach(var sprite in purgePartsSprites)
        {
            // プレファブを生成し、見た目（スプライト）を指定されたものに変更する
            var purgeParts = Instantiate(purgePartsPrefab, position, Quaternion.identity);
            purgeParts.GetComponent<SpriteRenderer>().sprite = sprite;
            // 初期設定を済ませる
            SetFirstSettings(ref purgeParts);
        }
    }
    public void AddPartsBySprite(Sprite purgePartsSprite)
    {
        if (purgePartsSprite == null) return;
        Vector3 position = transform.position;
        // プレファブを生成し、見た目（スプライト）を指定されたものに変更する
        var purgeParts = Instantiate(purgePartsPrefab, position, Quaternion.identity);
        purgeParts.GetComponent<SpriteRenderer>().sprite = purgePartsSprite;
        // 初期設定を済ませる
        SetFirstSettings(ref purgeParts);
    }

    // 生成したパージパーツオブジェクトに対して初期設定を行う
    private void SetFirstSettings(ref Rigidbody2D purgeParts)
    {
        // 初速度を設定する
        var quat = Quaternion.Euler(0, 0, Random.Range(0, initAngleRange) - initAngleRange * 0.5f);
        purgeParts.velocity = quat * Vector2.up * Random.Range(initMinVelocity, initMaxVelocity);
        purgeParts.angularVelocity = initAngleVelocity;
        // 指定秒数後にオブジェクトを破棄する
        Destroy(purgeParts.gameObject, DestroyDuration);
    }
}
