using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for moving a single bird
/// </summary>
public class BirdMover : MonoBehaviour
{

    public float Speed = 0.5f;

    private void FixedUpdate()
    {
        if (transform.eulerAngles.y == 270)
        {
            // the bird is facing left so move left
            transform.position = new Vector3(transform.position.x - Time.deltaTime * Speed, transform.position.y, transform.position.z);
        }
        else
        {
            // the bird is facing right so move right
            transform.position = new Vector3(transform.position.x + Time.deltaTime * Speed, transform.position.y, transform.position.z);
        }

    }
}
