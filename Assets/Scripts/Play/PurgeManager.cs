using System.Collections.Generic;
using UnityEngine;

// ���{�b�g�p�[�c�g�p��̂̃p�[�W��A�Q�[���I�[�o�[���̃��{�b�g�̃p�[�W�Ȃǂ̃p�[�W�I�u�W�F�N�g���Ǘ�����Component�B
public class PurgeManager : MonoBehaviour
{
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

    // �p�[�W����p�[�c���X�g�Ɏw��̃f�[�^��ǉ�����
    public void AddParts(List<Rigidbody2D> purgePartsGroup)
    {
        if (purgePartsGroup == null || purgePartsGroup.Count == 0) return;
        Vector3 position = transform.position;
        foreach (var prefab in purgePartsGroup)
        {
            // �I�u�W�F�N�g�𐶐����A�����x�Ȃǂ�ݒ肷��
            var rb = Instantiate(prefab, position + Vector3.back, Quaternion.identity);
            var quat = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, initAngleRange) - initAngleRange * 0.5f);
            rb.velocity = quat * Vector2.up * UnityEngine.Random.Range(initMinVelocity, initMaxVelocity);
            rb.angularVelocity = initAngleVelocity;
            // �w��b����ɔj������
            Destroy(rb.gameObject, DestroyDuration);
        }
    }
}
