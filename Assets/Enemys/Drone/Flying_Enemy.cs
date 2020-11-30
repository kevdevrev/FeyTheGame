using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Enemy : Enemy, IDamage
{
    [SerializeField] public int bulletDamage;
    [SerializeField] public int bulletSpeed;
    [SerializeField] private float fireRate = 0.9f;
    private float nextFire;
    [SerializeField] private GameObject beamPrefab;

    void Awake()
    {
        nextFire = Time.time;
    }
    
    private void CheckIfTimeToFire()
    {
        if (Time.time > nextFire)
        {
            Debug.Log("Shooting from enemy!");
            GameObject bullet = Instantiate(beamPrefab, transform.position, Quaternion.identity);
            bullet.name = beamPrefab.name;
            bullet.GetComponent<Bullet>().SetDamageValue(bulletDamage);
            bullet.GetComponent<Bullet>().SetBulletSpeed(bulletSpeed);
            //bullet.GetComponent<Bullet>().SetBulletDirection(new Vector2(_buddy_sprite.transform.localRotation.x, _buddy_sprite.transform.localRotation.y));
            bullet.GetComponent<Bullet>().Shoot();
            //Instantiate(beamPrefab, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
            anim.SetTrigger("AttackTrigger");

        }
    }

    public override void Init()
    {
        base.Init();
        Health = base.health;
    }

    public int Health { get; set; }

    public void Damage(int dmgTaken)
    {
        Debug.Log("Drone damaged:❤❤❤ " + dmgTaken);
        Health = Health - dmgTaken;
        anim.SetTrigger("Hit");
        rigid.AddForce(new Vector2(15f + rigid.mass, 15f + rigid.mass), ForceMode2D.Impulse);
        inCombat = true;
        anim.SetBool("InCombat", true);
        if (Health < 1)
        {
            anim.SetBool("Disabled", true);
            disabled = true;
        }
    }

}
