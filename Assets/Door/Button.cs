using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IDamage
{
    protected Animator anim;
    [SerializeField] protected GameObject target;
    
    public void Start()
    {
        anim = GetComponent<Animator>();

        Health = 1;
    }
    
    public int Health { get; set; }
    public void Damage(int dmgTaken)
    {
        Debug.Log("Hit Button");
        anim.SetTrigger("Button_Activate");
        IDamage hit = target.GetComponent<IDamage>();
        if (hit != null)
        {
            hit.Damage(1);
        }
    }
}
