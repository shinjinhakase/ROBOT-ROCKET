using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �p�[�c�A�C�R���ɔ����A�͂̕��������������Ǘ�����Component�B
public class IconDirectionForce : MonoBehaviour
{
    public void SetRotate(PartsInfo.PartsData data)
    {
        transform.rotation = Quaternion.Euler(0, 0, data.angle - 90);
    }
}
