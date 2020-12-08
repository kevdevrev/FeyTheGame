using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle2 : MonoBehaviour, IDamage
{
    // Start is called before the first frame update
    public Transform target;
    [SerializeField]
    protected int health;
    public float speed = 5f;
    public float rotateSpeed = 200f;
    [SerializeField] float destroyDelay;
    float destroyTime;
    [SerializeField] int damage = 1;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Fey").GetComponent<Transform>();
        destroyTime = destroyDelay;
        Health = health;
    }
	
    void FixedUpdate () {
        Vector2 direction = (Vector2)target.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.right * speed;
        
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
        {
            if (other.CompareTag("Fey") || other.CompareTag("Terrain") || other.CompareTag("EnemyBullet"))
            {
                Debug.Log("destroyed at fey / terrain");
                Destroy(gameObject);
            }else if(other.CompareTag("Buddy Bullet"))
            {
                Damage(1);
            }
            
        }
        
    }
    
    public int Health { get; set; }
    public void Damage(int dmgTaken)
    {
        Health = Health - dmgTaken;
        if(Health<1)
        {
            //play explosion animation?
            Destroy(gameObject);

        }    
    }
}
