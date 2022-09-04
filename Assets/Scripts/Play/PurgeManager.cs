using System.Collections.Generic;
using UnityEngine;

// ロボットパーツ使用後ののパージや、ゲームオーバー時のロボットのパージなどのパージオブジェクトを管理するComponent。
public class PurgeManager : MonoBehaviour
{
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

    // パージするパーツリストに指定のデータを追加する
    public void AddParts(List<Rigidbody2D> purgePartsGroup)
    {
        if (purgePartsGroup == null || purgePartsGroup.Count == 0) return;
        Vector3 position = transform.position;
        foreach (var prefab in purgePartsGroup)
        {
            // オブジェクトを生成し、初速度などを設定する
            var rb = Instantiate(prefab, position + Vector3.back, Quaternion.identity);
            var quat = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, initAngleRange) - initAngleRange * 0.5f);
            rb.velocity = quat * Vector2.up * UnityEngine.Random.Range(initMinVelocity, initMaxVelocity);
            rb.angularVelocity = initAngleVelocity;
            // 指定秒数後に破棄する
            Destroy(rb.gameObject, DestroyDuration);
        }
    }
}
