using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFloater : MonoBehaviour
{
    public float _amplitude = 0.05f;
    public float _speed = 5;
    private Vector3 _tempPos;
    
    void Start()
    {
        // store starting position
        _tempPos = transform.position;
    }

    private void FixedUpdate()
    {
        _tempPos = transform.position;

        // move along y axis, sinusoidally
        _tempPos.y = _tempPos.y + _amplitude * Mathf.Sin(_speed * Time.time);
        transform.position = _tempPos;
    }    
}
