using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyTrash : MonoBehaviour, IDamage
{
    protected Animator anim;
    
    public void Start()
    {
        anim = GetComponent<Animator>();

        Health = 1;
    }
    
    public int Health { get; set; }
    public void Damage(int dmgTaken)
    {
        Debug.Log("Ive been a door!");
        if (dmgTaken == -1)
        {
            anim.SetTrigger("Empty");
        }
    }
}
