using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトを振動させるComponent
public class Shaker : MonoBehaviour
{
    [SerializeField] private bool _doShake;
    public bool DoShake
    {
        get { return _doShake; }
        set {
            if( !_doShake && value)
            {
                _offsetX = Random.Range(0f, 100f);
                _offsetY = Random.Range(0f, 100f);
            }
            else if( _doShake && !value )
            {
                transform.localPosition = Vector3.zero;
            }
            _doShake = value;
        }
    }
    [SerializeField] private float _area = 1f;
    [SerializeField] private float _speed = 1f;

    private float _offsetX;
    private float _offsetY;

    // Start is called before the first frame update
    void Start()
    {
        _offsetX = Random.Range(0f, 100f);
        _offsetY = Random.Range(0f, 100f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!DoShake) return;

        Vector3 position = Vector3.zero;
        position.x += (2 * Mathf.PerlinNoise(_offsetX, Time.time * _speed) - 0.5f) * _area;
        position.y += (2 * Mathf.PerlinNoise(_offsetY, Time.time * _speed) - 0.5f) * _area;
        transform.localPosition = position;
    }
}
