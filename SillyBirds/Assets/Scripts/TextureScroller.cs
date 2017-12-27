using UnityEngine;
using System.Collections;

/// <summary>
/// Scrolls the texture of a material in a given direction
/// </summary>
public class TextureScroller : MonoBehaviour
{
    public float _speed;                         // how fast we want to texture to scroll 
    public bool _scrollVertically;               // the texture will scroll horizontally by default
    private Vector3 startPosition;
    private MeshRenderer meshRenderer;
    void Start()
    {
        startPosition = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void LateUpdate()
    {
        // offset the texture by a fraction 
        if(_scrollVertically)
        {
            meshRenderer.material.mainTextureOffset = new Vector2(0, Time.time * _speed);
        }
        else
        {
            meshRenderer.material.mainTextureOffset = new Vector2(Time.time * _speed, 0);
        }
        
    }
}