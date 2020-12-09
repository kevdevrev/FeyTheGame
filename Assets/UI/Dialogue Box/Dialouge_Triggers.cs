using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialouge_Triggers : MonoBehaviour
{
    [SerializeField] public GameObject target;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fey"))
        {
            Debug.Log("ACTIVATE DIALOG");
            target.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fey"))
        {
            Debug.Log("DEACTIVATE DIALOG");
            target.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
