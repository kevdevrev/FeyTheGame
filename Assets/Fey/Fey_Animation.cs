using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fey_Animation : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }
    
    
    public void animateJump(bool jumpCondition)
    {
        _animator.SetBool("Jump", jumpCondition);
    }

    public void animateAttack()
    {
        //find animation trigger
        _animator.SetTrigger("Attack");
        //play spin animation
        
    }
}
