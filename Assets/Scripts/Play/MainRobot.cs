using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �A�C�e�����g�p���ė͂������ē����A�Q�[���̃��C�����{�b�g
[RequireComponent(typeof(RobotStatus))]
[RequireComponent(typeof(ForceMove))]
public class MainRobot : MonoBehaviour
{
    RobotStatus _status;
    ForceMove _move;

    // �A�C�e���������I�Ɏg�p���邩�̃t���O�i���v���C�ȂǂŐ�����������Ȃ��悤�Ɂj
    private bool IsUsePartsInForce = false;
    // ���v���C����ɏ]����
    // private bool ReplayMode = false;


    private void Awake()
    {
        _status = GetComponent<RobotStatus>();
        _move = GetComponent<ForceMove>();
    }

    // Update is called once per frame
    private void Update()
    {
        // ���̓���e�X�g�����i�A�C�e���g�p�j
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseParts();
        }
    }

    // �Q�[���J�n���\�b�h
    [ContextMenu("Debug/GameStart")]
    public void GameStart()
    {
        // ���{�b�g�̏����d�ʂ�ݒ肷��
        float allWeight = PlayPartsManager.Instance.GetAllWeight();
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
        PartsInfo.PartsData data;
        IForce force;
        PlayPartsManager.Instance.UseParts(out data, out force);

        // ��ԊǗ��ɃA�C�e���̎g�p��`����
        _status.startUseParts(data);

        // �����Ǘ��ɗ͂�������
        _move.AddForce(force);
    }
}
