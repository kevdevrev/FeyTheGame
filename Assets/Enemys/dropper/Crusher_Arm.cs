using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher_Arm : MonoBehaviour
{
    public Crusher parent;


    private void Start()
    { 
        parent = gameObject.GetComponentInParent<Crusher>();
        //parent.Damage();
        
    }
}
