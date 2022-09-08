using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// リプレイデータを元に処理を呼び出すComponent
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ForceMove))]
public class ReplayPlayer : MonoBehaviour
{
    // 読み込んだリプレイデータ
    private bool IsLoaded = false;
    private bool IsPlaying = false;
    private bool IsEnd = false;
    private ReplayData _replayData;

    // 準備してきたパーツのIDリストを取得する（初期重量計算用）
    public List<PartsPerformance.E_PartsID> ReadyPartsIDList => _replayData.readyPartsList.ConvertAll(data => data.id);

    // 再生用変数
    private int frameCnt = 0;
    private int _readyPartsLength;  // 準備してきたパーツの数
    private int _startUseLength;    // 道中で獲得したパーツの数
    private int _endUseLength;      // パーツ使用終了フレームデータの配列長
    private int _transformLength;   // 座標更新データの配列長

    private int _currentPartsNo = 0;// 現在のパーツの使用数
    private int _currentTransNo = 0;// 現在の座標の更新回数

    // 呼び出す処理のイベント
    [Header("イベント系統")]
    [SerializeField] private UnityEvent<PartsInfo.PartsData> startUsePartsEvent = new UnityEvent<PartsInfo.PartsData>();
    [SerializeField] private UnityEvent endUsePartsEvent = new UnityEvent();
    [SerializeField] private UnityEvent<PartsInfo.PartsData> getPartsEvent = new UnityEvent<PartsInfo.PartsData>();
    [SerializeField] private UnityEvent endReplayEvent = new UnityEvent();

    // キャッシュ
    private Rigidbody2D _rb;
    private ForceMove _forceMove;

    void Awake()
    {
        _forceMove = GetComponent<ForceMove>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsPlaying && !IsEnd)
        {
            // 更新の有無を判定
            UpdateCheckPerFrame(out bool IsStartUsing, out bool IsEndUsing, out bool IsTransUpdate,
                out List<PartsInfo.PartsData> getPartsData, out List<ReplayData.ForceData> addForceList);

            // アイテムの獲得処理
            foreach(var partsData in getPartsData)
            {
                getPartsEvent.Invoke(partsData);
            }

            // アイテムの使用終了処理
            if (IsEndUsing)
            {
                endUsePartsEvent.Invoke();
            }

            // アイテムの使用開始処理
            if (IsStartUsing)
            {
                startUsePartsEvent.Invoke(GetPartsData);
                _currentPartsNo++;
            }

            // 位置情報の更新処理
            if (IsTransUpdate)
            {
                var locateData = GetLocateData;
                transform.position = locateData.position;
                _rb.velocity = locateData.velocity;
                _currentTransNo++;
            }

            // 力の追加処理
            foreach (var forceData in addForceList)
            {
                IForce force = forceData.buildForce();
                _forceMove.AddForce(force);
            }

            // リプレイの終了判定
            if(frameCnt >= _replayData.finishFrame)
            {
                IsEnd = true;
                endReplayEvent.Invoke();
            }
        }
    }

    // リプレイデータを読み込む
    public void LoadReplayData(int index)
    {
        // リプレイデータを取得する
        ReplayDatas replayDatas = ReplayDatas.Instance;
        if (index >= replayDatas.Length)
        {
            throw new Exception("リプレイデータが読み込めません。");
        }
        else if (IsLoaded)
        {
            throw new Exception("既にリプレイデータを読み込んでいます。");
        }
        IsLoaded = true;
        _replayData = replayDatas.GetData(index);

        // データのリスト長を取得
        _readyPartsLength = _replayData.readyPartsList.Count;
        _startUseLength = _replayData.usePartsFrame.Count;
        _endUseLength = _replayData.endUsePartsFrame.Count;
        _transformLength= _replayData.locateDatas.Count;
    }

    // リプレイを再生する
    public void StartReplay()
    {
        if (!IsLoaded)
        {
            throw new Exception("データを読み込んでいない状態ではリプレイを再生できません。");
        } else if (IsPlaying)
        {
            throw new Exception("既にリプレイの再生を開始しています。");
        }
        IsPlaying = true;
    }

    // 更新の有無を判定する
    private void UpdateCheckPerFrame(out bool IsStartUsing, out bool IsEndUsing, out bool IsTransUpdate,
        out List<PartsInfo.PartsData> getPartsData, out List<ReplayData.ForceData> addForceList)
    {
        IsStartUsing = _currentPartsNo < _startUseLength && frameCnt == _replayData.usePartsFrame[_currentPartsNo];
        IsEndUsing = _currentPartsNo != 0 && _currentPartsNo - 1 < _endUseLength && frameCnt == _replayData.endUsePartsFrame[_currentPartsNo - 1];
        IsTransUpdate = _currentTransNo < _transformLength && frameCnt >= _replayData.locateDatas[_currentTransNo].frame;
        getPartsData = _replayData.getPartsList.FindAll(data => frameCnt >= data.frame).ConvertAll(data => data.buildPartsData());
        addForceList = _replayData.forceDatas.FindAll(data => frameCnt == data.frame);
        frameCnt++;
    }

    // 使用するパーツのカスタムデータを取得する
    private PartsInfo.PartsData GetPartsData => _currentPartsNo < _readyPartsLength ?
        _replayData.readyPartsList[_currentPartsNo] : _replayData.getPartsList[_currentPartsNo - _readyPartsLength].buildPartsData();

    // 次の座標更新データを取得する
    private ReplayData.LocateData GetLocateData => _replayData.locateDatas[_currentTransNo];
}
