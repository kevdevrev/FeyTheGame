using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Drone : MonoBehaviour
{
    Rigidbody2D rb2d;
    SpriteRenderer sprite;

    protected Transform feyLocation;

    float destroyTime;

    public int damage = 1;

    [SerializeField] float bulletSpeed;
    [SerializeField] Vector2 bulletDirection;
    [SerializeField] float destroyDelay;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        feyLocation = GameObject.FindWithTag("Fey").GetComponent<Transform>();

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

    public void SetBulletDirection(Vector2 direction)
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
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0)
        {
            //_fey_sprite.flipX = false;
            bulletDirection.x = 1;
        }
        else if (horizontalInput < 0)
        {
            //_fey_sprite.flipX = true;
            bulletDirection.x = -1;
        }
        
        Vector2 moveDirection = bulletDirection * (feyLocation.transform.position - transform.position).normalized * bulletSpeed;
        // flip the bullet sprite for the highlight pixels
        //sprite.flipX = (bulletDirection.x < 0);
        // give it speed and how long it'll last
        rb2d.velocity = bulletDirection * bulletSpeed; 
            //new Vector2(moveDirection.x,moveDirection.y);
        destroyTime = destroyDelay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
        {
            if (other.CompareTag("Fey"))
            {
                Destroy(gameObject);
            }
            
        }
        
    }
}
