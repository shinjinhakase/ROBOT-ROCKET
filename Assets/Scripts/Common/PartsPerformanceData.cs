using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects")]
public class PartsPerformanceData : ScriptableObject
{
    public List<RobotPerformance> dataList = new List<RobotPerformance>();

    public RobotPerformance getPartsPerformance(E_PartsID id)
    {
        foreach(RobotPerformance performance in dataList)
        {
            if (performance.id == id) return performance;
        }
        throw new Exception("パーツID:" + id.ToString() + "の性能は定義されていません。");
    }

    // 力の加え方を定義する列挙型
    public enum E_ForceType
    {
        Bomb,           //爆弾。瞬間的な力を加える
        Rocket,         //ロケット。持続的で低加速・高速度な力を加える。
        Propeller,      //プロペラ。持続的で高加速・低速度な力を加える。
        Glider,         //グライダー。紙飛行機などのような特徴的な力を加える。
        CollisionForce  //特殊(ステージギミックなど)。プレイヤーが衝突している間、力を加え続ける。
    }
    // アイテムを区別するIDの役割を果たす列挙型
    public enum E_PartsID {
        TestParts
    }

    [Serializable]
    public class RobotPerformance
    {
        public E_PartsID id;
        public E_ForceType forceType;
        public float m; // 質量
        public float F; // 力
        public float t; // 時間
        public float v; // 終端速度
        public float R; // 抵抗力
        public float k; // 係数
    }
}
