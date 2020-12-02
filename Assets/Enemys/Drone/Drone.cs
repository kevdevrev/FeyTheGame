using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drone : Enemy, IDamage
{
    [SerializeField] public int bulletDamage;
    [SerializeField] public int bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootCooldown = 1f;
    private float shootCooldownTimer = 1f;
    private bool notOnCoolDown = true;
    private int counter;
    private float shootDelay = 0.3f;
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

    }

    protected override void Update()
    {
        base.Update();
        feyDirection = -1 * (transform.position - feyLocation.position).normalized;
        ShootTheBullet();
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

        }    
    }
    
    private void ShootTheBullet()
    {
        if (counter < bulletCount && shootDelayTimer < 0)
        {
            counter++;
            anim.SetTrigger("AttackTrigger");
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.name = bulletPrefab.name;
            bullet.GetComponent<Bullet>().SetDamageValue(bulletDamage);
            bullet.GetComponent<Bullet>().SetBulletSpeed(bulletSpeed);
            bullet.GetComponent<Bullet>().SetBulletDirection(feyDirection);
            bullet.GetComponent<Bullet>().Shoot();
            shootDelayTimer = shootDelay;
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
