using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxer : Enemy, IDamage
{



    public override void Init()
    {
        base.Init();
        Health = base.health;
    }

    public int Health { get; set; }
    public void Damage(int dmgTaken)
    {
        if (dmgTaken > 0)
        {
            Health = Health - dmgTaken;
            // if hit this is the hit response

            rigid.AddForce(new Vector2(15f + rigid.mass, 15f + rigid.mass), ForceMode2D.Impulse);
            inCombat = true;
            anim.SetBool("InCombat", true);
            if (Health < 1)
            {
                anim.SetTrigger("Liberated");
                disabled = true;
            }
        }
    }



}
