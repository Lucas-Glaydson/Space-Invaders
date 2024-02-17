using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    public AudioClip shootSound;
    public float speed = 200.0f;
    private bool _laserActive;
    private AudioSource _audioSource;
    private bool _isBorderLeft = false;
    private bool _isBorderRight = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) && !_isBorderLeft)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) && !_isBorderRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!_laserActive)
        {
            Projectile p = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            p.destroyed += LaserDestroyed;
            _laserActive = true;

            _audioSource.PlayOneShot(this.shootSound);
        }
    }

    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BorderLeft"))
            _isBorderLeft = true;

        else if (other.gameObject.layer == LayerMask.NameToLayer("BorderRight"))
            _isBorderRight = true;

        if (other.gameObject.layer == LayerMask.NameToLayer("NoBorderLeft"))
            _isBorderLeft = false;

        if (other.gameObject.layer == LayerMask.NameToLayer("NoBorderRight"))
            _isBorderRight = false;

        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") || other.gameObject.layer == LayerMask.NameToLayer("Missile"))
            Debug.Log("You lose!");
    }
}
