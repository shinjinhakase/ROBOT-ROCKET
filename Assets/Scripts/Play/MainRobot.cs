using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �A�C�e�����g�p���ė͂������ē����A�Q�[���̃��C�����{�b�g
[RequireComponent(typeof(RobotStatus))]
[RequireComponent(typeof(ForceMove))]
public class MainRobot : MonoBehaviour
{
    PartsInfo partsInfo;
    PlayPartsManager playPartsManager;
    RobotStatus _status;
    ForceMove _move;

    // �A�C�e���������I�Ɏg�p���邩�̃t���O�i���v���C�ȂǂŐ�����������Ȃ��悤�Ɂj
    private bool IsUsePartsInForce = false;
    // ���v���C����ɏ]����
    // private bool ReplayMode = false;


    private void Awake()
    {
        partsInfo = PartsInfo.Instance;
        _status = GetComponent<RobotStatus>();
        _move = GetComponent<ForceMove>();
    }

    // Update is called once per frame
    private void Update()
    {
        // ��s���̏���
        if (_status.IsFlying)
        {
            // �A�C�e���g�p�I������
            if (_status.IsUsingParts && !playPartsManager.IsUsingParts) _status.endUseParts();

            // ���̑��쏈���i�A�C�e���g�p�j
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("�A�C�e���g�p�{�^����������");
                UseParts();
            }
            // �A�C�e���̎蓮�p�[�W�i�O���C�_�[�ł͎����H���̃p�[�c�ł͂ǂ����邩�����ĂȂ��j
            if(playPartsManager.IsUsingParts && Input.GetKeyDown(KeyCode.R))
            {
                playPartsManager.IsUsingParts = false;
            }
        }

        // �q�b�g�X�g�b�v�f�o�b�O
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlaySceneController.Instance.RequestHitStopBySlow(0.25f, 1f);
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            PlaySceneController.Instance.RequestHitStopByStop(1f);
        }
    }

    // �Q�[���J�n���\�b�h
    [ContextMenu("Debug/GameStart")]
    public void GameStart()
    {
        // ���{�b�g�̏����d�ʂ�ݒ肷��
        playPartsManager = PlayPartsManager.Instance;
        float allWeight = playPartsManager.GetAllWeight();
        _move.SetWeight(allWeight + ForceMove.RobotWeight);

        // ��Ԃ�ω�������
        _status.startGame();
    }

    // �A�C�e�����g�p���鏈��
    [ContextMenu("Debug/UseParts")]
    public void UseParts()
    {
        // �A�C�e�����g���邩����
        if (!_status.IsFlying)
        {
            Debug.LogWarning("��s���ȊO�ɃA�C�e�����g�p���悤�Ƃ��Ă��܂��I");
            return;
        }
        else if (!partsInfo.HasNext) return;
        else if (!_status.IsPartsUsable)
        {
            // �A�C�e�����g�p�ł��Ȃ���Ԃ̂Ƃ�
            if (!IsUsePartsInForce) return;
            else
            {
                // �A�C�e�����g�p�ł����Ԃɋ����I�Ɉڍs����
                if (_status.IsUsingParts) _status.endUseParts();
                _status.endCooltime();
            }
        }

        // �A�C�e���Ǘ��ɃA�C�e���̎g�p��`���A�K�v�ȏ���Ⴄ
        PartsPerformance performance;
        PartsInfo.PartsData data;
        IForce force;
        playPartsManager.UseParts(out performance, out data, out force);

        // ��ԊǗ��ɃA�C�e���̎g�p��`����
        _status.startUseParts(performance, data);

        // �����Ǘ��ɗ͂�������
        _move.AddForce(force);

        // �����I�u�W�F�N�g����������
        Transform _transform = transform;
        Vector3 nowPosition = _transform.position;
        foreach(SummonableObject summonObject in performance.summonObjects)
        {
            var summonned = Instantiate(summonObject, nowPosition, Quaternion.identity);
            summonned.Summon(data, _transform);
        }
    }
}