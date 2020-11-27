using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Type : Enemy, IDamage
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
        if(Health<1)
        {
            Debug.Log("I died!");
            anim.SetBool("Disabled",true);
            disabled = true;
            Debug.Log("I am now disabled maybe? " + disabled);
            //glove.GetComponent<SpriteRenderer>().enabled=true;

        }
    }
}
