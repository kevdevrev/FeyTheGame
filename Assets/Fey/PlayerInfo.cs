using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance;
    public int health;
    public bool hasBuddy;
    public bool wasDead;
    void Awake ()
    {
        DontDestroyOnLoad(this.gameObject);
        /*if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }*/
        
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Duplicate GameManager created every time the scene is loaded
            Destroy(gameObject);
        }
    }
}
