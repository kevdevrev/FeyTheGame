using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class F_Type : Enemy, IDamage
{

    /*[SerializeField] protected Material blockMaterial;
    [SerializeField] protected Material liberatedMaterial;
    [SerializeField] protected Material spinMaterial;*/
    private Light2D F_Type_Light;
    
    public override void Init()
    {
        base.Init();
        Health = base.health;
    }
    void Start()
    {
        base.Init();
        Health = base.health;
        anim = GetComponentInChildren<Animator>();
        F_Type_Light = transform.GetChild(0).GetComponent<Light2D>();
        //materialReference = transform.GetComponent<Renderer>();

    }

    protected void Update(){
        F_Type_Light.lightCookieSprite = sprite.sprite;
        base.Update();

}

    public int Health { get; set; }
    public void Damage(int dmgTaken)
    {
        if (dmgTaken > 0)
        {
            //materialReference.material = blockMaterial;
            Health = Health - dmgTaken;
            anim.SetTrigger("Hit");
            rigid.AddForce(new Vector2(15f + rigid.mass, 15f + rigid.mass), ForceMode2D.Impulse);
            inCombat = true;
            anim.SetBool("InCombat", true);
            if (Health < 1)
            {
                anim.SetBool("Disabled", true);
                disabled = true;
                //materialReference.material = liberatedMaterial;

            }
        }
    }

    protected override void Attack()
    {
        base.Attack();
        //Debug.Log("material switch");
        //materialReference.material = spinMaterial;
    }
}
