using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] public int damageDealt;
    //cooldown before object can take damage again

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.CompareTag("Player") 
              && this.CompareTag("Player"))  
            && !(other.CompareTag("Player") && this.CompareTag("Fey")) 
            && !(other.CompareTag("Fey") 
                 && this.CompareTag("Player"))
            && !(other.CompareTag("Fey") 
                 && this.CompareTag("Fey"))
            && !(other.CompareTag("Buddy Bullet") 
                 && this.CompareTag("Buddy Bullet"))
            && !(other.CompareTag("Buddy Bullet") 
                 && this.CompareTag("Fey"))
            && !(other.CompareTag("Fey") 
                 && this.CompareTag("Buddy Bullet"))
            && !(other.CompareTag("Enemy") && this.CompareTag("Enemy"))
            && !(other.CompareTag("EnemyBullet") && this.CompareTag("EnemyBullet"))
            && !(other.CompareTag("EnemyBullet") && this.CompareTag("Enemy"))
            && !(other.CompareTag("Enemy") && this.CompareTag("EnemyBullet"))
            
            )
        {
            IDamage hit = other.GetComponent<IDamage>();
            if (hit != null)
            {
                if (this.CompareTag("Button"))
                {
                    Debug.Log(this);
                    hit.Damage(1);
                }
                else if (other.CompareTag("Fey") && other.gameObject.layer != 10)
                {
                    Debug.Log("fey hurted");
                    Fey fey = other.GetComponent<Fey>();
                    if (!fey._immunity)
                    {
                        hit.Damage(damageDealt);
                        fey.tookDamage();
                    }
                }
                else if (other.CompareTag("Enemy"))
                { 
                    Enemy enemy = other.GetComponent<Enemy>();
                    if (!enemy._immunity)
                    {
                        hit.Damage(damageDealt);
                        enemy.tookDamage();
                    }
                }
                else if (other.CompareTag("EnemyBullet"))
                {
                    hit.Damage(damageDealt);
                }
                else
                {
                    hit.Damage(damageDealt);
                }
            }

            /*if (hit != null){
                if (this.CompareTag("Button"))
                {
                    Debug.Log(this);
                    hit.Damage(1);
                }
                else
                    {
                        if (damageDealt > 0)
                        {
                            //Debug.Log("hit: " + other.name);
                            //call our interface in order to access its methods related to the object this is attached to
                            //should be attached to the fight hitbox.
                            if (_immunity == false)
                            {
                                hit.Damage(damageDealt);

                                _immunity = true;
                                StartCoroutine(EnemyImmunityCoolDown());
                            }
                        }
                    }
                }*/
            }

    } 
    
}
