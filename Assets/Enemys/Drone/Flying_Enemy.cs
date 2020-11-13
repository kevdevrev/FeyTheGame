using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Enemy : Enemy, IDamage
{

    public override void Init()
    {
        base.Init();
        Health = base.health;
    }

    public int Health { get; set; }

    public void Damage(int dmgTaken)
    {
        Debug.Log("I took " + dmgTaken);
        Health = Health - dmgTaken;
        anim.SetTrigger("Hit");
        rigid.AddForce(new Vector2(15f + rigid.mass, 15f + rigid.mass), ForceMode2D.Impulse);
        inCombat = true;
        anim.SetBool("InCombat", true);
        if (Health < 1)
        {
            Debug.Log("Idied!");
            anim.SetBool("Disabled", true);
            disabled = true;
            Debug.Log("I am now disabled maybe? " + disabled);
            //glove.GetComponent<SpriteRenderer>().enabled=true;

        }
    }

    public override void WayPointLogic()
    {
        //check to see if Fey is far away enough to justify walking
        float feys_X_DistanceAway = Mathf.Abs(feyLocation.position.x - transform.position.x);
        float feys_Y_DistanceAway = Mathf.Abs(feyLocation.position.y - transform.position.y);


        if (feys_X_DistanceAway > maxXDistanceAway && feys_Y_DistanceAway < maxYDistanceAway)
        {
            inCombat = false;
            anim.SetBool("InCombat", false);
        }
        else if (feys_X_DistanceAway < maxXDistanceAway && feys_X_DistanceAway > enemyAttackRange &&
                 feys_Y_DistanceAway < maxYDistanceAway)
        {
            anim.SetBool("Chase", true);
            inCombat = true;
            //tells it to hit
            anim.SetBool("InCombat", false);
            //Debug.Log("Fey is nearby lets chase her");
            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                float desiredY = transform.position.y;
                if (transform.position.y < feyLocation.position.y)
                {
                    desiredY = Random.Range(1f,4f) + feyLocation.position.y;
                }
                

                Vector3 feyCurLocation =
                    new Vector3(feyLocation.position.x, desiredY, transform.position.z);
                transform.position =
                    Vector3.MoveTowards(transform.position, feyCurLocation, speed * Time.deltaTime);
            }
        }
        else if (feys_X_DistanceAway < enemyAttackRange && feys_Y_DistanceAway < maxYDistanceAway)
        {
            anim.SetBool("Chase", false);
            anim.SetBool("InCombat", true);
            if (transform.position.y < feyLocation.position.y)
            {
                float desiredY = Random.Range(0.1f,4f) + feyLocation.position.y;
                Vector3 feyCurLocation =
                    new Vector3(feyLocation.position.x, desiredY, transform.position.z);
                transform.position =
                    Vector3.MoveTowards(transform.position, feyCurLocation, speed * Time.deltaTime);
            }
            else if (transform.position.y > feyLocation.position.y + 2f)
            {
                float desiredY = Random.Range(0.1f,2f) + feyLocation.position.y;
                Vector3 feyCurLocation =
                    new Vector3(feyLocation.position.x, desiredY, transform.position.z);
                transform.position =
                    Vector3.MoveTowards(transform.position, feyCurLocation, speed * Time.deltaTime);
            }
        }

        //no 360 no scope
        if (anim.GetBool("Chase") || anim.GetBool("InCombat"))
        {
            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                float facingDirection = feyLocation.position.x - transform.position.x;
                if (facingDirection > 0)
                {
                    sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (facingDirection < 0)
                {
                    sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }

        }
    }
}
