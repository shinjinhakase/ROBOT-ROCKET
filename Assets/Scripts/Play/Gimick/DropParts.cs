using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DropParts : GimickBase
{
    // �擾����p�[�c�̏��
    [SerializeField] private PartsInfo.PartsData partsData;
    private bool Picked = false;

    // �M�~�b�N�����Z�b�g���郁�\�b�h
    public override void ResetGimick() {
        gameObject.SetActive(true);
        Picked = false;
    }

    // ���̓����蔻��ƏՓ˂����ۂ̃A�C�e�����E������
    public void PickItem(Collider2D other)
    {
        if (!other.TryGetComponent(out MainRobot robot) || Picked) return;
        Picked = true;
        // ���{�b�g�̏d�ʂ𑝂₵�A�g�p�p�[�c���X�g�̈�ԍŌ�Ɋl���p�[�c��ǉ�����
        PlayPartsManager.Instance.GetParts(partsData, out PartsPerformance performance);
        ForceMove move = robot._move;
        move.SetWeight(move.GetWeight() + performance.m);

        // ���g�𖳌�������
        gameObject.SetActive(false);
    }
}
