using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigid_bullet;

    private SpriteRenderer bullet_sprite;

    private float destroyTime;

    public int damage = 1;

    [SerializeField] private float bulletSpeed;
    [SerializeField] private Vector2 bulletDirection;
    [SerializeField] private float destroyDelay;
    
    
    void Awake()
    {
        rigid_bullet = GetComponent<Rigidbody2D>();
        bullet_sprite = GetComponent<SpriteRenderer>();
        
        
    }
    
    void Update()
    {
        // maybe make into corutine
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetBulletSpeed(float speed)
    {
        this.bulletSpeed = speed;
    }

    public void SetBulletDirection(Vector2 direction)
    {
        this.bulletDirection = direction;
    }

    public void SetDamageValue(int damage)
    {
        this.damage = damage;
    }

    public void SetDestroyOnDelay(float delay)
    {
        this.destroyDelay = delay;
    }

    public void Shoot()
    {
        //if our bullet is traveling left, we do this to flip the sprite
        bullet_sprite.flipX = (bulletDirection.x < 0);
        rigid_bullet.velocity = bulletDirection * bulletSpeed;
        destroyTime = destroyDelay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}
