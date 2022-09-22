using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// パーツアイコンに伴い、力の方向を示す矢印を管理するComponent。
public class IconDirectionForce : MonoBehaviour
{
    public void SetRotate(PartsInfo.PartsData data)
    {
        transform.rotation = Quaternion.Euler(0, 0, data.angle - 90);
    }
}
