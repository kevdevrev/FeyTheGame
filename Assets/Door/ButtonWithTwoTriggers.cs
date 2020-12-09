using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWithTwoTriggers : MonoBehaviour, IDamage
{
    protected Animator anim;
    [SerializeField] protected GameObject[] targets;
    private int numTargets;
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
        for(int i = 0; i < targets.Length; i++)
        {
            IDamage hit = targets[i].GetComponent<IDamage>();
            if (hit != null)
            {
                hit.Damage(-1);
            } 
        }
    }
}
