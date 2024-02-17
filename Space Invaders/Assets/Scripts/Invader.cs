using System;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public float animationTime = 1.0f;
    public Action<int> invaderDestroyed;
    public Sprite deathSprite;
    public int row;
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {           
            this.invaderDestroyed.Invoke(row);
            _spriteRenderer.sprite = deathSprite;
            this.gameObject.SetActive(false);
        }
    }
}
