using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Buddy : MonoBehaviour
{
    [SerializeField] protected Material blinkMaterial;
    [SerializeField] protected Material attackMaterial;
    private Light2D _buddy_Light;
    public Renderer materialReference;
    [SerializeField] protected Transform castPoint;
    [SerializeField] protected float aggroTimeLimit;
    [SerializeField]protected float aggroTimer;
    [SerializeField] protected bool isAggrod;

    [SerializeField] public int bulletDamage;
    [SerializeField] public int bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootCooldown = 1f;
    private float shootCooldownTimer = 1f;
    private bool notOnCoolDown = true;
    private int counter;
    protected Animator anim;
    protected SpriteRenderer _buddy_sprite;
    [SerializeField]protected Vector2 enemyDistanceAwayVector;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        _buddy_sprite = GetComponentInChildren<SpriteRenderer>();
        _buddy_Light = transform.GetChild(0).GetComponent<Light2D>();
        materialReference = transform.GetComponent<Renderer>();
        counter = 0;
        aggroTimer = aggroTimeLimit;

    }

    // Update is called once per frame
    private void Update()
    {
        _buddy_Light.lightCookieSprite = _buddy_sprite.sprite;
        ShootTheBullet();
        
        if (isAggrod == true)
        {
            aggroTimer -= Time.deltaTime;
            Debug.Log("aggrod: true");
            if (aggroTimer < 0)
            {
                Debug.Log("aggrod: false");
                aggroTimer = aggroTimeLimit;
                isAggrod = false;
                anim.SetBool("InCombat", false);
                //anim.SetTrigger("MoveLeft");
            }
        }
    }

    /*private void DetectIfEnemyNearby(GameObject[] enemys)
    {
        if (!anim.GetBool("InCombat"))
        {
            foreach (GameObject e in enemys)
            {
                float xCondition = e.transform.position.x - transform.position.x;
                float yCondition = e.transform.position.y - transform.position.y;
                float dist = Vector2.Distance(e.transform.position, transform.position);
                if (dist < 3)
                {
                    Debug.Log("DIST LESS THAN 3");
                    //TODO shoot a raycast first or change to collider detection
                    if (CanSeeEnemy(e.transform))
                    {
                        Debug.Log("SAW ENEMY");
                        anim.SetBool("InCombat", true);
                        anim.SetTrigger("MoveRight");
                        isAggrod = true;

                    }

                    return;
                }
            }
        }

        anim.SetBool("InCombat", false);
        return;
    }*/

    public bool CanSeeEnemy(Transform enemyLocation)
    {
        float direction;
        enemyDistanceAwayVector = enemyLocation.position - transform.position;
        if (enemyDistanceAwayVector.x > 0)
            {
                direction = -1;

            }
            else if (enemyDistanceAwayVector.x < 0)
            {
                direction = 1;
            }

            //Vector2 endPos = castPoint.position + direction * (Vector3.right * aggroDistance);
        //RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << 9)
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, enemyLocation.position, (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Floor")));
        
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.tag);

            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("hit enemy");
                Debug.DrawLine(castPoint.position,hit.point, Color.green);
                return true;
            }
            else
            {
                Debug.Log("hit not enemy");
                Debug.DrawLine(castPoint.position,hit.point, Color.red);
                return false;
            }
        }
        else
        {
            Debug.DrawLine(castPoint.position, enemyLocation.position, Color.blue);
        }
        
        return false;
    }
    
    
    private void ShootTheBullet()
    {
        if (counter < 5)
        {
            if (Input.GetMouseButtonDown(1))
            {
                
                materialReference.material = attackMaterial;
                anim.SetTrigger("MoveRight");
                anim.ResetTrigger("MoveRight");
                //StartCoroutine(ResetInCombatStatus());
                anim.SetTrigger("Attack");
                if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("buddy_combat"))
                {
                    counter++;
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    bullet.name = bulletPrefab.name;
                    bullet.GetComponent<Bullet>().SetDamageValue(bulletDamage);
                    bullet.GetComponent<Bullet>().SetBulletSpeed(bulletSpeed);
                    //Debug.Log(new Vector2(_buddy_sprite.transform.localRotation.x, _buddy_sprite.transform.localRotation.y));

                    bullet.GetComponent<Bullet>().SetBulletDirection(transform.right);
                    bullet.GetComponent<Bullet>().Shoot();
                }

                //Debug.Log("Shooting!");
            }
        }
        else if(counter >= 5 && notOnCoolDown == true)
        {
            shootCooldownTimer = shootCooldown;
            notOnCoolDown = false;
            materialReference.material = blinkMaterial;
        }
        else
        {
            shootCooldownTimer -= Time.deltaTime;
            if (shootCooldownTimer < 0)
            {
                shootCooldownTimer = 3;
                counter = 0; notOnCoolDown = true;
            }
        }
    }
    
    IEnumerator ResetInCombatStatus()
    {
        //anim.ResetTrigger("AttackTrigger");
        Debug.Log("In coruten");
        yield return new WaitForSeconds(4f);
        anim.SetBool("InCombat", false);

    }    
    IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(3f);
        counter = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (CanSeeEnemy(other.transform))
            {
                isAggrod = true;
                anim.SetBool("InCombat", true);
                anim.SetTrigger("MoveRight");
            }
        }
    }
}
