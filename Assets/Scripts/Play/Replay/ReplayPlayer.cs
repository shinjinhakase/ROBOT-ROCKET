using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ���v���C�f�[�^�����ɏ������Ăяo��Component
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ForceMove))]
public class ReplayPlayer : MonoBehaviour
{
    // �ǂݍ��񂾃��v���C�f�[�^
    private bool _isLoaded = false;
    public bool IsLoaded { get { return _isLoaded; } private set { _isLoaded = value; } }
    private bool _isPlaying = false;
    public bool IsPlaying { get { return _isPlaying; } private set { _isPlaying = value; } }
    private ReplayData _replayData;

    // �������Ă����p�[�c��ID���X�g���擾����i�����d�ʌv�Z�p�j
    public List<PartsPerformance.E_PartsID> ReadyPartsIDList => _replayData.readyPartsList.ConvertAll(data => data.id);
    public List<PartsInfo.PartsData> InitialPartsDatas => _replayData.readyPartsList;

    // �Đ��p�ϐ�
    private int frameCnt = 0;
    private int _readyPartsLength;  // �������Ă����p�[�c�̐�
    private int _startUseLength;    // �����Ŋl�������p�[�c�̐�
    private int _endUseLength;      // �p�[�c�g�p�I���t���[���f�[�^�̔z��
    private int _transformLength;   // ���W�X�V�f�[�^�̔z��

    private int _currentPartsNo = 0;// ���݂̃p�[�c�̎g�p��
    private int _currentTransNo = 0;// ���݂̍��W�̍X�V��

    // �Ăяo�������̃C�x���g
    [Header("�C�x���g�n��")]
    [SerializeField] private UnityEvent onLoadEvent = new UnityEvent();
    [SerializeField] private UnityEvent<PartsInfo.PartsData> startUsePartsEvent = new UnityEvent<PartsInfo.PartsData>();
    [SerializeField] private UnityEvent endUsePartsEvent = new UnityEvent();
    [SerializeField] private UnityEvent<PartsInfo.PartsData> getPartsEvent = new UnityEvent<PartsInfo.PartsData>();
    [SerializeField] private UnityEvent endReplayEvent = new UnityEvent();

    // �L���b�V��
    private Rigidbody2D _rb;
    private ForceMove _forceMove;

    void Awake()
    {
        _forceMove = GetComponent<ForceMove>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isPlaying)
        {
            // �X�V�̗L���𔻒�
            UpdateCheckPerFrame(out bool IsStartUsing, out bool IsEndUsing, out bool IsTransUpdate,
                out List<PartsInfo.PartsData> getPartsData, out List<ReplayData.ForceData> addForceList);

            // �A�C�e���̊l������
            foreach(var partsData in getPartsData)
            {
                getPartsEvent.Invoke(partsData);
            }

            // �A�C�e���̎g�p�I������
            if (IsEndUsing)
            {
                endUsePartsEvent.Invoke();
            }

            // �A�C�e���̎g�p�J�n����
            if (IsStartUsing)
            {
                try
                {
                    PartsInfo.PartsData getParts = GetPartsData;
                    startUsePartsEvent.Invoke(getParts);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new Exception("���v���C�f�[�^����g�p�����p�[�c�̐��\���擾�ł��܂���B");
                }
                _currentPartsNo++;
            }

            // �ʒu���̍X�V����
            if (IsTransUpdate)
            {
                var locateData = GetLocateData;
                transform.position = locateData.position;
                _rb.velocity = locateData.velocity;
                _currentTransNo++;
            }

            // �͂̒ǉ�����
            foreach (var forceData in addForceList)
            {
                IForce force = forceData.buildForce();
                _forceMove.AddForce(force);
            }

            // ���v���C�̏I������
            if(frameCnt >= _replayData.finishFrame)
            {
                _isLoaded = false;
                _isPlaying = false;
                endReplayEvent.Invoke();
            }
        }
    }

    // ���v���C�f�[�^��ǂݍ���
    public void LoadReplayData(ReplayData data)
    {
        // ���v���C�f�[�^���擾����
        if (_isLoaded)
        {
            throw new Exception("���Ƀ��v���C�f�[�^��ǂݍ���ł��܂��B");
        }
        _isLoaded = true;
        _replayData = data;

        // �f�[�^�̃��X�g�����擾
        _readyPartsLength = _replayData.readyPartsList.Count;
        _startUseLength = _replayData.usePartsFrame.Count;
        _endUseLength = _replayData.endUsePartsFrame.Count;
        _transformLength= _replayData.locateDatas.Count;

        // �J�E���^��������
        frameCnt = 0;
        _currentPartsNo = 0;
        _currentTransNo = 0;

        onLoadEvent.Invoke();
    }

    // ���v���C���Đ�����
    public void StartReplay()
    {
        if (!_isLoaded)
        {
            throw new Exception("�f�[�^��ǂݍ���ł��Ȃ���Ԃł̓��v���C���Đ��ł��܂���B");
        } else if (_isPlaying)
        {
            throw new Exception("���Ƀ��v���C�̍Đ����J�n���Ă��܂��B");
        }
        _isPlaying = true;
    }

    // �X�V�̗L���𔻒肷��
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

    // �g�p����p�[�c�̃J�X�^���f�[�^���擾����
    private PartsInfo.PartsData GetPartsData => _currentPartsNo < _readyPartsLength ?
        _replayData.readyPartsList[_currentPartsNo] : _replayData.getPartsList[_currentPartsNo - _readyPartsLength].buildPartsData();

    // ���̍��W�X�V�f�[�^���擾����
    private ReplayData.LocateData GetLocateData => _replayData.locateDatas[_currentTransNo];

    // �J�n���̏������ʂ�ݒ肷��
    public float GetInitialWeight(PlayPartsManager _playPartsManager)
    {
        float sumWeight = 0;
        foreach (var readyPartsID in ReadyPartsIDList)
        {
            sumWeight += _playPartsManager.GetPerformance(readyPartsID).m;
        }
        return sumWeight + ForceMove.RobotWeight;
    }
}
