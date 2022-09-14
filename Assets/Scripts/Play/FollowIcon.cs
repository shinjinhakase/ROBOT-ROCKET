using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �I�u�W�F�N�g��Ǐ]���铮�������Component
public class FollowIcon : MonoBehaviour
{
    public Transform target; // �ǔ��Ώ�
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float Duration = 0.2f; // ����

    private WaitForSeconds _waitForSeconds;

    void Awake()
    {
        _waitForSeconds = new WaitForSeconds(Duration);
    }

    void FixedUpdate()
    {
        // �擾�������W���w��b����ɃZ�b�g����
        if (target != null) StartCoroutine(UpdatePosition(target.position));
    }

    // �w��b����Ɏw��̍��W�ɍ��킹�郁�\�b�h
    private IEnumerator UpdatePosition(Vector3 position)
    {
        yield return _waitForSeconds;
        transform.position = position;
    }

    // �j�����\�b�h
    public void DestroyMyself()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    // �X�v���C�g�ύX���\�b�h
    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
