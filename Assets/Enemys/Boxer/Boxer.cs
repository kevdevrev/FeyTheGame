using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxer : Enemy, IDamage
{
    [SerializeField] protected Material hurtMaterial;
    [SerializeField] protected Material prevMaterial;
    [SerializeField] protected float materialTimer = 1f;


    public override void Init()
    {
        base.Init();
        Health = base.health;
    }

    public int Health { get; set; }
    public void Damage(int dmgTaken)
    {
        Health = Health - dmgTaken;
        // if hit this is the hit response
        prevMaterial = sprite.material;
        sprite.material = hurtMaterial;
        rigid.AddForce(new Vector2(15f + rigid.mass, 15f + rigid.mass), ForceMode2D.Impulse);
        inCombat = true;
        anim.SetBool("InCombat", true);
        if(Health<1)
        {
            anim.SetTrigger("Liberated");
            disabled = true;
        }
        else
        {
            StartCoroutine(ResetMaterial());
        }
    }
    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(materialTimer);
        sprite.material = prevMaterial;
    }
    


}
