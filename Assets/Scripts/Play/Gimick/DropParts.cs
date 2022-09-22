using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IconBox))]
public class DropParts : PickableGimick
{
    private IconBox _iconBox;

    // �擾����p�[�c�̏��
    [SerializeField] private PartsInfo.PartsData partsData;

    public override void Awake()
    {
        base.Awake();
        _iconBox = GetComponent<IconBox>();
    }

    public override void OnSceneStart()
    {
        StartCoroutine(SetIconSprite());
    }

    protected override void PickedActionOnlyPlay(MainRobot mainRobot)
    {
        // ���{�b�g�̏d�ʂ𑝂₵�A�g�p�p�[�c���X�g�̈�ԍŌ�Ɋl���p�[�c��ǉ�����
        PlayPartsManager.Instance.GetParts(partsData, out PartsPerformance performance);
        mainRobot._move.AddWeightByPartsPerformance(performance);
    }

    // �A�C�R���̃X�v���C�g��ݒ肷��
    public IEnumerator SetIconSprite()
    {
        while (true)
        {
            PlayPartsManager _instance = PlayPartsManager.Instance;
            if (_instance)
            {
                _iconBox.SetSprite(_instance.GetPerformance(partsData.id), partsData);
                yield break;
            }
            yield return null;
        }
    }
}
