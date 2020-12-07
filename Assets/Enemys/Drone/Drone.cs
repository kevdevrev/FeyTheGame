using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;


public class Drone : Enemy, IDamage
{
    [SerializeField] protected Material idleMaterial;
    [SerializeField] protected Material attackMaterial;
    [SerializeField] protected Material liberatedMaterial;
    private Light2D _drone_Light;
    public SpriteRenderer drone_sprite;
    public Renderer materialReference;
    
    
    
    [SerializeField] public int bulletDamage;
    [SerializeField] public int bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootCooldown = 1f;
    private float shootCooldownTimer = 1f;
    private bool notOnCoolDown = true;
    private int counter;
    //private float shootDelay = 0.3f;
    private float shootDelayTimer = 0.3f;
    [SerializeField] public int bulletCount = 5;

    [SerializeField] private Vector2 feyDirection;
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        Health = base.health;
        anim = GetComponentInChildren<Animator>();
        counter = 0;
        drone_sprite = transform.GetComponent<SpriteRenderer>();
        //Get the animation script handler
        _drone_Light = transform.GetChild(1).GetComponent<Light2D>();
        materialReference = transform.GetComponent<Renderer>();
    }

    protected override void Update()
    {
        _drone_Light.lightCookieSprite = drone_sprite.sprite;
        if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            materialReference.material = idleMaterial;
        }

        feyDirection = -1 * (transform.position - feyLocation.position).normalized;
        if (!disabled)
        {
            //if idle, we want to prevent movement, so we do nothing, so just return
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                return;
            }

            MovementLogic();
        }
    }

    protected void MovementLogic()
    {
        //check to see if Fey is far away enough to justify walking
        feyDistanceAwayVector = feyLocation.position - transform.position;
        //WayPointLogic();
        if (!isAggrod && (!CanSeePlayer() || (Mathf.Abs(feyDistanceAwayVector.x) > detectionRadius.x
                             || Mathf.Abs(feyDistanceAwayVector.y) > detectionRadius.y)))
        {
            //Debug.Log("WayPointMode");
            inCombat = false;
            anim.SetBool("InCombat", false);
            anim.SetTrigger("Move");
            anim.SetBool("WayPointMode", true);
            destination = WayPointLogic(destination);
        }
        else
        {
            
            isAggrod = true;
            aggroTimer -= Time.deltaTime;
            if (aggroTimer < 0)
            {
                aggroTimer = aggroTimeLimit;
                isAggrod = false;
            }

            if (CanSeePlayer())
            {
                float facingDirection = feyLocation.position.x - transform.position.x;
                Debug.Log(facingDirection);
                if (facingDirection > 0)
                {
                    sprite.flipX = false;
                        //sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    isFacingLeft = false;
                }
                else if (facingDirection < 0)
                {
                    sprite.flipX = true;
                        //sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    isFacingLeft = true;
                }

                anim.SetBool("WayPointMode", false);

                //handle chasing to get into hit distance
                //Debug.Log(Mathf.Abs(feyDistanceAwayVector.x) > enemyAttackRange);
                if (Mathf.Abs(feyDistanceAwayVector.x) < detectionRadius.x
                    && Mathf.Abs(feyDistanceAwayVector.y) < detectionRadius.y
                    && Mathf.Abs(feyDistanceAwayVector.x) > enemyAttackRange)
                {
                    Debug.Log("In Chase Mode");
                    anim.SetBool("Chase", true);
                    inCombat = true;
                    //tells it to hit
                    anim.SetBool("InCombat", false);
                    //Debug.Log("Fey is nearby lets chase her");
                    if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        Vector2 targetLocation = new Vector2(feyLocation.position.x, transform.position.y);
                        transform.position =
                            Vector2.MoveTowards(transform.position, targetLocation, speed * Time.deltaTime);
                    }
                }
                else if (Mathf.Abs(feyDistanceAwayVector.x) < enemyAttackRange)
                {
                    anim.SetBool("Chase", false);
                    anim.SetBool("InCombat", true);

                    anim.SetTrigger("Move");
                    if (feyDistanceAwayVector.x > 0)
                    {
                        sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        isFacingLeft = false;
                    }
                    else if (feyDistanceAwayVector.x < 0)
                    {
                        sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        isFacingLeft = true;
                    }
                    
                    if (CanSeePlayer())
                    {
                        ShootTheBullet();
                    }
                }
                else
                {
                    if (destination == pointA.position)
                    {
                        if (sprite.transform.localRotation != Quaternion.Euler(0, 180, 0))
                        {
                            sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                            isFacingLeft = true;
                        }
                    }
                    else if (destination == pointB.position)
                    {
                        if (sprite.transform.localRotation != Quaternion.Euler(0, 0, 0))
                        {
                            sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            isFacingLeft = false;
                        }

                    }
                }
            }
        }

    }
    public int Health { get; set; }
    public void Damage(int dmgTaken)
    {
        Health = Health - dmgTaken;
        anim.SetTrigger("Hit");
        rigid.AddForce(new Vector2(15f + rigid.mass, 15f + rigid.mass), ForceMode2D.Impulse);
        inCombat = true;
        anim.SetBool("InCombat", true);
        if(Health<1)
        {
            anim.SetBool("Disabled",true);
            disabled = true;
            materialReference.material = liberatedMaterial;

        }    
    }
    
    private void ShootTheBullet()
    {
        if (counter < bulletCount && shootDelayTimer < 0)
        {
            counter++;
            anim.SetTrigger("AttackTrigger");
            materialReference.material = attackMaterial;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.name = bulletPrefab.name;
            bullet.GetComponent<Bullet>().SetDamageValue(bulletDamage);
            bullet.GetComponent<Bullet>().SetBulletSpeed(bulletSpeed);
            bullet.GetComponent<Bullet>().SetBulletDirection(feyDirection);
            bullet.GetComponent<Bullet>().Shoot();
            shootDelayTimer = attackCooldownTimer;
        }
        else if(counter >= bulletCount && notOnCoolDown == true)
        {
            shootCooldownTimer = shootCooldown;
            notOnCoolDown = false;
        }
        else if(shootDelayTimer > 0 && notOnCoolDown == true)
        {
            shootDelayTimer -= Time.deltaTime;
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
}
