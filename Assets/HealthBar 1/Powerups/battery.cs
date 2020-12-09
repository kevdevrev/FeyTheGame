using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battery : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        StartCoroutine(DestroyPowerUp());
    }
    
    IEnumerator DestroyPowerUp()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);

    }
}
