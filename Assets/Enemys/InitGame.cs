using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    public Fey fey;
    // Start is called before the first frame update
    void Awake()
    {
        
        fey = GameObject.FindWithTag("Fey").GetComponent<Fey>();
        PlayerPrefs.SetInt("wasDead", true?1:0);
        PlayerPrefs.SetInt("hasBuddy", false?1:0);
        PlayerPrefs.SetInt("Health", fey.Health);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
