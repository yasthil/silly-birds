using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdRotator : MonoBehaviour
{

    public float Speed = 0.5f;

    private void Update()
    {
        transform.Rotate(Vector3.up * (Speed * Time.deltaTime));
    }
}
