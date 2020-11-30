using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb2d;
    SpriteRenderer sprite;

    float destroyTime;

    public int damage = 1;

    [SerializeField] float bulletSpeed;
    [SerializeField] Vector3 bulletDirection;
    [SerializeField] float destroyDelay;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // remove this bullet once its time is up
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetBulletSpeed(float speed)
    {
        // set bullet speed
        this.bulletSpeed = speed;
    }

    public void SetBulletDirection(Vector3 direction)
    {
        // set bullet direction vector
        this.bulletDirection = direction;
    }

    public void SetDamageValue(int damage)
    {
        // how much damage does this bullet cause
        this.damage = damage;
    }

    public void SetDestroyDelay(float delay)
    {
        // the time this bullet will last if it doesn't collide
        this.destroyDelay = delay;
    }

    public void Shoot()
    {
        rb2d.velocity = bulletDirection * bulletSpeed;
        destroyTime = destroyDelay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Fey"))
        {
            if (other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
            
        }
        
    }
}
