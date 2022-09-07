using System.Collections.Generic;
using UnityEngine;

// ���{�b�g�p�[�c�g�p��̂̃p�[�W��A�Q�[���I�[�o�[���̃��{�b�g�̃p�[�W�Ȃǂ̃p�[�W�I�u�W�F�N�g���Ǘ�����Component�B
public class PurgeManager : MonoBehaviour
{
    [Tooltip("�p�[�W����p�[�c�̊�b�ƂȂ�v���t�@�u�B�X�v���C�g�݂̂��w�肳�ꂽ�ꍇ�A���̌����ڂ������ύX�����B")]
    [SerializeField] private Rigidbody2D purgePartsPrefab;

    [Header("�p�[�W���̏����ݒ�")]
    [Tooltip("���ł���܂ł̎���")]
    [SerializeField] private float DestroyDuration = 2f;
    [Tooltip("�ŏ��̏����x")]
    [SerializeField] private float initMaxVelocity = 4f;
    [Tooltip("�ő�̏����x")]
    [SerializeField] private float initMinVelocity = 2f;
    [Tooltip("������������̊p�x��")]
    [SerializeField] private float initAngleRange = 120f;
    [Tooltip("��]�̊p���x")]
    [SerializeField] private float initAngleVelocity = 0f;

    // �p�[�W����p�[�c���X�g�Ɏw��̃f�[�^���v���t�@�u���w�肵�Ēǉ�����
    public void AddPartsByPrefab(List<Rigidbody2D> purgePartsGroup)
    {
        if (purgePartsGroup == null || purgePartsGroup.Count == 0) return;
        Vector3 position = transform.position;
        foreach (var prefab in purgePartsGroup)
        {
            // �I�u�W�F�N�g�𐶐����A�����x�Ȃǂ�ݒ肷��
            var purgeParts = Instantiate(prefab, position, Quaternion.identity);
            // �����ݒ���ς܂���
            SetFirstSettings(ref purgeParts);
        }
    }
    public void AddPartsByPrefab(Rigidbody2D purgePartsPrefab)
    {
        if (purgePartsPrefab == null ) return;
        Vector3 position = transform.position;
        // �I�u�W�F�N�g�𐶐����A�����x�Ȃǂ�ݒ肷��
        var purgeParts = Instantiate(purgePartsPrefab, position, Quaternion.identity);
        // �����ݒ���ς܂���
        SetFirstSettings(ref purgeParts);
    }

    // �p�[�W����p�[�c���X�g�Ɏw��̃f�[�^���X�v���C�g����\�z���Ēǉ�����
    public void AddPartsBySprite(List<Sprite> purgePartsSprites)
    {
        if (!(purgePartsSprites?.Count > 0)) return;
        Vector3 position = transform.position;
        foreach(var sprite in purgePartsSprites)
        {
            // �v���t�@�u�𐶐����A�����ځi�X�v���C�g�j���w�肳�ꂽ���̂ɕύX����
            var purgeParts = Instantiate(purgePartsPrefab, position, Quaternion.identity);
            purgeParts.GetComponent<SpriteRenderer>().sprite = sprite;
            // �����ݒ���ς܂���
            SetFirstSettings(ref purgeParts);
        }
    }
    public void AddPartsBySprite(Sprite purgePartsSprite)
    {
        if (purgePartsSprite == null) return;
        Vector3 position = transform.position;
        // �v���t�@�u�𐶐����A�����ځi�X�v���C�g�j���w�肳�ꂽ���̂ɕύX����
        var purgeParts = Instantiate(purgePartsPrefab, position, Quaternion.identity);
        purgeParts.GetComponent<SpriteRenderer>().sprite = purgePartsSprite;
        // �����ݒ���ς܂���
        SetFirstSettings(ref purgeParts);
    }

    // ���������p�[�W�p�[�c�I�u�W�F�N�g�ɑ΂��ď����ݒ���s��
    private void SetFirstSettings(ref Rigidbody2D purgeParts)
    {
        // �����x��ݒ肷��
        var quat = Quaternion.Euler(0, 0, Random.Range(0, initAngleRange) - initAngleRange * 0.5f);
        purgeParts.velocity = quat * Vector2.up * Random.Range(initMinVelocity, initMaxVelocity);
        purgeParts.angularVelocity = initAngleVelocity;
        // �w��b����ɃI�u�W�F�N�g��j������
        Destroy(purgeParts.gameObject, DestroyDuration);
    }
}
