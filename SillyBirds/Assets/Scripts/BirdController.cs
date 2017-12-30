using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public GameObject _explosion;
    public Color _particleColor;
    public AudioClip _explodeSound;

    /// <summary>
    /// When a bird leaves the boundary, don't destroy it - set it to inactive
    /// This is where object pooling comes in
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            if (gameObject.tag == "Bird")
            {
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void Explode()
    {
        // play explode sound
        PlayExplodeSound();

        // play burst animation
        Vector3 pos = transform.position;
     
        // offset the explosion so it looks centered
        pos.y += 1;

        // play explosion particle effect
        GameObject explosion = Instantiate(_explosion, pos, Quaternion.identity);
        explosion.GetComponent<ParticleSystemRenderer>().material.color = _particleColor;
    }

    private void PlayExplodeSound()
    {
        // create an empty GameObject to attach the sound to
        GameObject soundContainerGO = new GameObject("ObstacleExplodeSoundContainer");
        AudioSource audioSource = soundContainerGO.AddComponent<AudioSource>();       
        audioSource.clip = _explodeSound;
        audioSource.Play();
    }

}
