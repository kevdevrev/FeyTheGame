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
    
    
    [SerializeField] public int bulletDamage;
    [SerializeField] public int bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootCooldown = 1f;
    private float shootCooldownTimer = 1f;
    private bool notOnCoolDown = true;
    private int counter;
    protected Animator anim;
    protected SpriteRenderer _buddy_sprite;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        _buddy_sprite = GetComponentInChildren<SpriteRenderer>();
        _buddy_Light = transform.GetChild(0).GetComponent<Light2D>();
        materialReference = transform.GetComponent<Renderer>();
        counter = 0;

    }

    // Update is called once per frame
    private void Update()
    {
        _buddy_Light.lightCookieSprite = _buddy_sprite.sprite;
        ShootTheBullet();
    }

    private void DetectIfEnemyNearby(GameObject[] enemys)
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
                    //TODO shoot a raycast first or change to collider detection
                    anim.SetBool("InCombat", true);
                    anim.SetTrigger("MoveRight");
                    
                    return;
                }
            }
        }

        anim.SetBool("InCombat", false);
        return;
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
            anim.SetBool("InCombat", true);
            anim.SetTrigger("MoveRight");
        }
    }
}
