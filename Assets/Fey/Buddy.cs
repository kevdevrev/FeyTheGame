using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buddy : MonoBehaviour
{
    [SerializeField] public int bulletDamage;
    [SerializeField] public int bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    protected Animator anim;
    protected SpriteRenderer _buddy_sprite;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        _buddy_sprite = GetComponentInChildren<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        ShootTheBullet();
        DetectIfEnemyNearby(enemys);
    }

    private GameObject[] DetectIfEnemyNearby(GameObject[] enemys)
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
                    anim.SetBool("InCombat", true);
                    anim.SetTrigger("MoveRight");
                    return enemys;
                }
            }
        }

        anim.SetBool("InCombat", false);
        return enemys;
    }

    private void ShootTheBullet()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.C))
        {
            anim.SetTrigger("MoveRight");
            anim.ResetTrigger("MoveRight");
            //StartCoroutine(ResetInCombatStatus());
            anim.SetTrigger("Attack");
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.name = bulletPrefab.name;
            bullet.GetComponent<Bullet>().SetDamageValue(bulletDamage);
            bullet.GetComponent<Bullet>().SetBulletSpeed(bulletSpeed);
            //bullet.GetComponent<Bullet>().SetBulletDirection(new Vector2(_buddy_sprite.transform.localRotation.x, _buddy_sprite.transform.localRotation.y));
            bullet.GetComponent<Bullet>().Shoot();
            Debug.Log("Shooting!");
        }
    }
    
    IEnumerator ResetInCombatStatus()
    {
        //anim.ResetTrigger("AttackTrigger");
        Debug.Log("In coruten");
        yield return new WaitForSeconds(4f);
        anim.SetBool("InCombat", false);

    }
}
