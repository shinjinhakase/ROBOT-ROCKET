using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeScrollbar : MonoBehaviour
{
    [SerializeField] private float size = 0.1f;

    private ScrollRect scrollRect;

    // Start is called before the first frame update
    void Start()
    {
        //scrollRect = GetComponent<ScrollRect>();
        //OnResize();
    }

    public void OnResize()
    {
        //scrollRect.verticalScrollbar.size = size;
    }
}
