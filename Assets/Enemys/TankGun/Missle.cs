using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public Transform feyLocation;
    [SerializeField] public float rotateSpeed;


/*public Transform feyLocation;
private Rigidbody2D rigid;
[SerializeField] public float missleSpeed;
[SerializeField] public float rotateSpeed;
SpriteRenderer sprite;
private Vector2 missleDirection;
float destroyTime;

[SerializeField] int damage = 1;
[SerializeField] float destroyDelay;

// Start is called before the first frame update
void Awake()
{
    sprite = GetComponent<SpriteRenderer>();
    feyLocation = GameObject.FindWithTag("Fey").GetComponent<Transform>();
    rigid = GetComponent<Rigidbody2D>();
    Debug.Log(damage);
    Debug.Log(missleSpeed);
}

public void SetBulletSpeed(float speed)
{
    // set bullet speed
    this.missleSpeed = speed;
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
    missleDirection = (Vector2)feyLocation.position - rigid.position;
    Debug.Log("Missle direction " + missleDirection);
    missleDirection.Normalize();
    float rotateAmount = Vector3.Cross(missleDirection, transform.right).z;

    rigid.angularVelocity = -rotateAmount * rotateSpeed;
    rigid.velocity = transform.right * missleSpeed;
    
    destroyTime = destroyDelay;
}

private void OnTriggerEnter2D(Collider2D other)
{
    if (!other.CompareTag("Enemy"))
    {
        if (other.CompareTag("Fey") || other.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
        
    }
    
}

void FixedUpdate()
{
    Debug.Log(damage);
    Debug.Log(missleSpeed);
    // remove this bullet once its time is up
    destroyTime -= Time.deltaTime;
    if (destroyTime < 0)
    {
        Destroy(gameObject);
    }
    
    /*
    ////////////////
    Vector2 direction = (Vector2)feyLocation.position - rigid.position;
    direction.Normalize();
    float rotateAmount = Vector3.Cross(direction, transform.right).z;

    rigid.angularVelocity = -rotateAmount * rotateSpeed;
    rigid.velocity = transform.right * speed;#1#
}*/
    
    Rigidbody2D rb2d;
    SpriteRenderer sprite;

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
        feyLocation = GameObject.FindWithTag("Fey").GetComponent<Transform>();
        Debug.Log("working");
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

        bulletDirection = (Vector2)feyLocation.position - rb2d.position;
        bulletDirection.Normalize();
        float rotateAmount = Vector3.Cross(bulletDirection, transform.right).z;

        rb2d.angularVelocity = -rotateAmount * rotateSpeed;
        rb2d.velocity = transform.right * bulletSpeed;
        // flip the bullet sprite for the highlight pixels
        //sprite.flipX = (bulletDirection.x < 0);
        // give it speed and how long it'll last
        /*Vector2 direction = (Vector2)feyLocation.position - rb2d.position;
        direction.Normalize();
        bulletDirection = direction;
        rb2d.velocity = bulletDirection * bulletSpeed;*/
        destroyTime = destroyDelay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
        {
            if (other.CompareTag("Fey") || other.CompareTag("Terrain"))
            {
                Destroy(gameObject);
            }
            
        }
        
    }
}
