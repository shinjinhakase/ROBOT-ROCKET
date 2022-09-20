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
    // アイテムの使用終了タイミングのリスト
    public List<int> endUsePartsFrame = new List<int>();

    // ロボットの座標データのリスト
    public List<LocateData> locateDatas = new List<LocateData>();
    // ロボットに加わった力のリスト
    public List<ForceData> forceDatas = new List<ForceData>();

    // ゲーム終了タイミングと記録
    public int finishFrame = 0;
    public float score = 0f;
    public Collector.CollectionDataList stageCollectionDatas = Collector.CollectionDataList.Build();
    public Collector.CollectionDataList getCollectionDatas = Collector.CollectionDataList.Build();

    public ReplayData() { }
    // コピーコンストラクタ
    public ReplayData(ReplayData originalData)
    {
        StageNum = originalData.StageNum;
        readyPartsList = new List<PartsInfo.PartsData>(originalData.readyPartsList);
        getPartsList = new List<GetPartsData>(originalData.getPartsList);
        usePartsFrame = new List<int>(originalData.usePartsFrame);
        endUsePartsFrame = new List<int>(originalData.endUsePartsFrame);
        locateDatas = new List<LocateData>(originalData.locateDatas);
        forceDatas = new List<ForceData>(originalData.forceDatas);
        finishFrame = originalData.finishFrame;
        score = originalData.score;
        stageCollectionDatas = originalData.stageCollectionDatas.Clone();
        getCollectionDatas = originalData.getCollectionDatas.Clone();
    }

    // データを初期化して、準備したパーツデータを読み込む
    public void ReadyPartsInfo(int StageNum)
    {
        this.StageNum = StageNum;
        readyPartsList = new List<PartsInfo.PartsData>(PartsInfo.Instance.GetPartsList());
        getPartsList.Clear();
        usePartsFrame.Clear();
        endUsePartsFrame.Clear();
        locateDatas.Clear();
        forceDatas.Clear();
        finishFrame = 0;
        score = 0f;
        stageCollectionDatas.Clear();
        getCollectionDatas.Clear();
    }
    // パーツの使用タイミングを記録する
    public void RegisterUseParts(int frame)
    {
        usePartsFrame.Add(frame);
    }
    // パーツの使用終了タイミングを記録する
    public void RegisterEndUseParts(int frame)
    {
        endUsePartsFrame.Add(frame);
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
        // 力を加え始めたフレームを計算する
        frame = frame - force.frameCnt + 1;
        // それぞれの力のタイプに合わせてデータを記録する
        if (typeof(PressForce) == force.GetType())
        {
            PressForce pressForce = (PressForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.Rocket,
                pressForce.Angle, t: force.frameCnt - 1, F: pressForce.F, k: pressForce.k, m: pressForce.m);
            forceDatas.Add(forceData);
        }
        else if (typeof(ImpulseForce) == force.GetType())
        {
            ImpulseForce impulseForce = (ImpulseForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.Bomb,
                impulseForce.Angle, F: impulseForce.F, m: impulseForce.m);
            forceDatas.Add(forceData);
        }
        else if (typeof(GliderForce) == force.GetType())
        {
            GliderForce gliderForce = (GliderForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.Glider,
                gliderForce.Angle, t: force.frameCnt - 1, F: gliderForce.F, k: gliderForce.R, m: gliderForce.m);
            forceDatas.Add(forceData);
        }
        else if (typeof(CollisionForce) == force.GetType())
        {
            // 時間の対応がそもそもまだ
            CollisionForce collisionForce = (CollisionForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.CollisionForce,
                collisionForce.Angle, t: force.frameCnt - 1, F: collisionForce.F, k: collisionForce.k, m: collisionForce.m);
            forceDatas.Add(forceData);
        }
        else
        {
            throw new Exception("加わった力をリプレイに保存できませんでした。");
        }
    }
    // 終了結果を記録する
    public void RegisterResult(int frame, float score)
    {
        finishFrame = frame;
        this.score = score;
    }
    // 収集要素の獲得状況を記録する
    public void RegisterCollector(Collector.CollectionDataList stageDatas, Collector.CollectionDataList getDatas)
    {
        stageCollectionDatas = stageDatas;
        getCollectionDatas = getDatas;
    }

    // ロボットの位置情報を格納するクラス
    [Serializable]
    public struct LocateData
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
    public struct ForceData
    {
        public int frame;   // 経過フレーム数
        public PartsPerformance.E_ForceType forceType;
        public float t; // 時間（秒）
        public float Angle;
        public float F;
        public float k;
        public float m;
        public ForceData(int frame, PartsPerformance.E_ForceType forceType, float Angle,
            int t = 0, float F = 0, float k = 0, float m = 0)
        {
            this.frame = frame;
            this.forceType = forceType;
            this.t = t * Time.fixedDeltaTime;
            this.Angle = Angle;
            this.F = F;
            this.k = k;
            this.m = m;
        }
        // データを元に力を構築する
        public IForce buildForce()
        {
            switch (forceType)
            {
                // 瞬間的な力は角度とFだけで再現可能
                case PartsPerformance.E_ForceType.Bomb:
                    return new ImpulseForce(Angle, F, m);
                // 継続的な力はパージしたタイミングを元に構築する
                // 当たり判定についても力が加わった期間が分かっているので、PressForceで再現する
                case PartsPerformance.E_ForceType.Rocket:
                case PartsPerformance.E_ForceType.Propeller:
                case PartsPerformance.E_ForceType.CollisionForce:
                    return new PressForce(Angle, F, t, k, m);
                // グライダーは引数Rの情報をフィールドkに格納しているので、kでインスタンス化する
                case PartsPerformance.E_ForceType.Glider:
                    return new GliderForce(Angle, F, t, k, m);
                // 力無しは対応していないのでエラー
                case PartsPerformance.E_ForceType.NoForce:
                default:
                    throw new Exception("リプレイ用の力の構築に失敗しました。");
            }
        }
    }
    // 獲得したパーツデータを格納するクラス
    [Serializable]
    public struct GetPartsData
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
