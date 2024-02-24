using UnityEngine;

public class Bunker : MonoBehaviour
{
    public Sprite[] healthSprites;
    
    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            this.gameObject.SetActive(false);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            _animationFrame++;

            if (_animationFrame >= this.healthSprites.Length) 
            {
                this.gameObject.SetActive(false);
                _animationFrame = 0;
            }

            _spriteRenderer.sprite = this.healthSprites[_animationFrame];
        }
    }
}
