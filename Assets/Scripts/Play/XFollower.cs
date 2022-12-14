using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// X軸方向に追尾するComponent
public class XFollower : MonoBehaviour
{
    public Transform target;

    private void FixedUpdate()
    {
        if (target)
        {
            Vector3 nowPosition = transform.position;
            nowPosition.x = target.position.x;
            transform.position = nowPosition;
        }
    }
}
