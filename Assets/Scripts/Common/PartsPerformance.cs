using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Parts", menuName = "ScriptableObjects/PartsPerformance")]
public class PartsPerformance : ScriptableObject
{
    // 力の加え方を定義する列挙型
    public enum E_ForceType
    {
        Bomb,           //爆弾。瞬間的な力を加える
        Rocket,         //ロケット。持続的で低加速・高速度な力を加える。
        Propeller,      //プロペラ。持続的で高加速・低速度な力を加える。
        Glider,         //グライダー。紙飛行機などのような特徴的な力を加える。
        CollisionForce, //特殊(ステージギミックなど)。プレイヤーが衝突している間、力を加え続ける。
        NoForce         //攻撃用のアイテムなど。特に力を加えない。
    }
    // アイテムを区別するIDの役割を果たす列挙型
    public enum E_PartsID
    {
        TestParts,
        Rocket1,
        Rocket2,
        Rocket3
    }

    // 物理等プレイシーン関連
    public E_PartsID id;
    public E_ForceType forceType;
    public float m; // 質量
    public float F; // 力
    public float t; // 時間
    public float R; // 抵抗力
    public float k; // 係数
    public float cooltime;  // 効果終了後のクールタイム
    public List<SummonableObject> summonObjects = new List<SummonableObject>(); //召喚オブジェクトリスト

    // UI関連項目
    public string partsName;    //アイテム名前
    [Tooltip("装備時に表示される見た目。")]
    public Sprite partsSprite;
    [Tooltip("UI用にアイコンとして用いられる見た目。")]
    public Sprite iconSprite;
    [Tooltip("説明文。UIでパーツ説明に用います。"), TextArea(3, 5)]
    public string description;  //アイテム説明
}
