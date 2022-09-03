using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// そのプレイのリプレイ用データを格納するクラス。（ここでのframeとは、FixedUpdateの処理単位時間でのこと）
[Serializable]
public class ReplayData
{
    // ステージ番号
    public int StageNum = -1;
    // 事前に準備してきたパーツのリスト
    public List<PartsInfo.PartsData> readyPartsList = new List<PartsInfo.PartsData>();
    // 道中で獲得したパーツのリスト
    public List<GetPartsData> getPartsList = new List<GetPartsData>();
    // アイテムの使用タイミングのリスト
    public List<int> usePartsFrame = new List<int>();

    // ロボットの座標データのリスト
    public List<LocateData> locateDatas = new List<LocateData>();
    // ロボットに加わった力のリスト
    public List<ForceData> forceDatas = new List<ForceData>();

    // ゲーム終了タイミングと記録
    public int finishFrame = 0;
    public float score = 0f;


    // データを初期化して、準備したパーツデータを読み込む
    public void ReadyPartsInfo(int StageNum)
    {
        this.StageNum = StageNum;
        readyPartsList = new List<PartsInfo.PartsData>(PartsInfo.Instance.GetPartsList());
        getPartsList.Clear();
        usePartsFrame.Clear();
        locateDatas.Clear();
        forceDatas.Clear();
        finishFrame = 0;
        score = 0f;
    }
    // パーツの使用タイミングを記録する
    public void RegisterUseParts(int frame)
    {
        usePartsFrame.Add(frame);
    }
    // 獲得したパーツデータを記録する
    public void RegisterGetParts(int frame, PartsPerformance.E_PartsID id, float Angle)
    {
        getPartsList.Add(new GetPartsData(frame, id, Angle));
    }
    // ロボットの座標データを記録する
    public void RegisterRobotTransform(int frame, Vector2 position, Vector2 velocity)
    {
        locateDatas.Add(new LocateData(frame, position, velocity));
    }
    // ロボットに加わった力を記録する
    public void RegisterRobotForce(int frame, IForce force)
    {
        // TODO：途中で手動パージした場合の時間の対応、当たり判定型の力の対応
        Debug.LogWarning("力のリプレイデータ保存機能はまだ完全に実装できていません。");
        if (typeof(PressForce) == force.GetType())
        {
            PressForce pressForce = (PressForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.Rocket,
                pressForce.Angle, t: pressForce.t, F: pressForce.F, k: pressForce.k);
            forceDatas.Add(forceData);
        }
        else if (typeof(ImpulseForce) == force.GetType())
        {
            ImpulseForce impulseForce = (ImpulseForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.Bomb,
                impulseForce.Angle, F: impulseForce.F);
            forceDatas.Add(forceData);
        }
        else if (typeof(GliderForce) == force.GetType())
        {
            GliderForce gliderForce = (GliderForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.Glider,
                gliderForce.Angle, t: gliderForce.t, F: gliderForce.F, k: gliderForce.R);
            forceDatas.Add(forceData);
        }
        else if (typeof(CollisionForce) == force.GetType())
        {
            // 時間の対応がそもそもまだ
            CollisionForce collisionForce = (CollisionForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.CollisionForce,
                collisionForce.Angle, t: 0, F: collisionForce.F, k: collisionForce.k);
            forceDatas.Add(forceData);
        }
        throw new Exception("加わった力をリプレイに保存できませんでした。");
    }
    // 終了結果を記録する
    public void RegisterResult(int frame, float score)
    {
        finishFrame = frame;
        this.score = score;
    }

    // ロボットの位置情報を格納するクラス
    [Serializable]
    public class LocateData
    {
        public int frame;   // 経過フレーム数
        public Vector2 position;    // 座標
        public Vector2 velocity;    // 速度
        public LocateData(int frame, Vector2 position, Vector2 velocity)
        {
            this.frame = frame;
            this.position = position;
            this.velocity = velocity;
        }
    }
    // 加わった力を記録するクラス
    [Serializable]
    public class ForceData
    {
        public int frame;   // 経過フレーム数
        public PartsPerformance.E_ForceType forceType;
        public float t; // 時間（秒）
        public float Angle;
        public float F;
        public float k;
        public ForceData(int frame, PartsPerformance.E_ForceType forceType, float Angle,
            float t = 0, float F = 0, float k = 0)
        {
            this.frame = frame;
            this.forceType = forceType;
            this.t = t;
            this.F = F;
            this.k = k;
        }
        // データを元に力を構築する
        public IForce buildForce()
        {
            switch (forceType)
            {
                // 瞬間的な力は角度とFだけで再現可能
                case PartsPerformance.E_ForceType.Bomb:
                    return new ImpulseForce(Angle, F);
                // 継続的な力はパージしたタイミングを元に構築する
                // 当たり判定についても力が加わった期間が分かっているので、PressForceで再現する
                case PartsPerformance.E_ForceType.Rocket:
                case PartsPerformance.E_ForceType.Propeller:
                case PartsPerformance.E_ForceType.CollisionForce:
                    return new PressForce(Angle, F, t, k);
                // グライダーは引数Rの情報をフィールドkに格納しているので、kでインスタンス化する
                case PartsPerformance.E_ForceType.Glider:
                    return new GliderForce(Angle, F, t, k);
                // 力無しは対応していないのでエラー
                case PartsPerformance.E_ForceType.NoForce:
                default:
                    throw new Exception("リプレイ用の力の構築に失敗しました。");
            }
        }
    }
    // 獲得したパーツデータを格納するクラス
    [Serializable]
    public class GetPartsData
    {
        public int frame;
        public PartsPerformance.E_PartsID id;
        public float Angle;
        public GetPartsData(int frame, PartsPerformance.E_PartsID id, float Angle)
        {
            this.frame = frame;
            this.id = id;
            this.Angle = Angle;
        }
        // PartsInfo用のパーツデータを構築する
        public PartsInfo.PartsData buildPartsData()
        {
            var data = new PartsInfo.PartsData();
            data.id = id;
            data.angle = Angle;
            return data;
        }
    }
}
