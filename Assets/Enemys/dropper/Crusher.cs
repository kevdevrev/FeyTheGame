using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Crusher : MonoBehaviour, IDamage
{
    public Renderer materialReference;
    [SerializeField] protected Material hurtMaterial;
    [SerializeField] protected Material liberatedMaterial;
    [SerializeField] protected Material prevMaterial;
    
    [SerializeField]
    protected int health;
    protected Animator anim;
    protected SpriteRenderer sprite;
    private Light2D Crusher_Light;
    protected bool disabled;
    protected bool attackOnCooldown = false;
    protected float attackCooldownTimer = 1f;
    [SerializeField] protected float materialTimer = 1f;
    private float invinceTimer;
    [SerializeField] private float invinceCooldown;
    private bool notOnInvince;
    
    
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        disabled = false;
        //materialReference = transform.GetComponent<Renderer>();
        Health = health;
        invinceTimer = invinceCooldown;
    }
    
    protected virtual void Update()
    {

        if (notOnInvince == false)
        {
            //Crusher_Light.lightCookieSprite = sprite.sprite;
            invinceTimer -= Time.deltaTime;
            if (invinceTimer < 0)
            {
                invinceTimer = invinceCooldown;
                notOnInvince = true;
            }
        }
    }
    


    public int Health { get; set; }
    public void Damage(int dmgTaken)
    {
        if (notOnInvince == true)
        {
            Debug.Log("Im been hit and im a CRUSEHR");
            Health = Health - dmgTaken;
            prevMaterial = sprite.material;
            sprite.material = hurtMaterial;

            //anim.SetTrigger("Hit");
            if (Health < 1)
            {
                sprite.material = liberatedMaterial;
                anim.SetBool("Disabled", true);
                disabled = true;
            }
            else
            {
                StartCoroutine(ResetMaterial());
                notOnInvince = false;
            }
        }
    }
    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(materialTimer);
        sprite.material = prevMaterial;
    }
}


