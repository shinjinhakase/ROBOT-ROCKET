using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DropParts : GimickBase
{
    [SerializeField] private SpriteRenderer _iconSpriteRenderer;
    [SerializeField] private IconDirectionForce _iconDirectionForce;

    // �擾����p�[�c�̏��
    [SerializeField] private PartsInfo.PartsData partsData;
    private bool Picked = false;

    public override void OnSceneStart()
    {
        if (_iconSpriteRenderer) StartCoroutine(SetIconSprite());
        if (_iconDirectionForce) _iconDirectionForce.SetRotate(partsData);
    }

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

    // �A�C�R���̃X�v���C�g��ݒ肷��
    public IEnumerator SetIconSprite()
    {
        while (true)
        {
            PlayPartsManager _instance = PlayPartsManager.Instance;
            if (_instance)
            {
                _iconSpriteRenderer.sprite = _instance.GetPerformance(partsData.id).iconSprite;
                yield break;
            }
            yield return null;
        }
    }
}
