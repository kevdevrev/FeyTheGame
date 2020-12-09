using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySupportScript : MonoBehaviour
{
    public Enemy parent;
    private void Start()
    { 
        parent = gameObject.GetComponentInParent<Enemy>();
        
        //parent.Damage();
        
    }
    

    
}
