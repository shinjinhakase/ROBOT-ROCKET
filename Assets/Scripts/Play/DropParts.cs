using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DropParts : MonoBehaviour
{
    // �擾����p�[�c�̏��
    [SerializeField] private PartsInfo.PartsData partsData;

    // ���̓����蔻��ƏՓ˂����ۂ̃A�C�e�����E������
    public void PickItem(Collider2D other)
    {
        if (!other.TryGetComponent(out MainRobot robot)) return;
        // ���{�b�g�̏d�ʂ𑝂₵�A�g�p�p�[�c���X�g�̈�ԍŌ�Ɋl���p�[�c��ǉ�����
        PlayPartsManager.Instance.GetParts(partsData, out PartsPerformance performance);
        ForceMove move = robot._move;
        move.SetWeight(move.GetWeight() + performance.m);

        // ���g��j������
        Destroy(gameObject);
    }
}
