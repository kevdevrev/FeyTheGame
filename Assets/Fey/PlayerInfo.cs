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
        wasDead = true;
        hasBuddy = false;
        health = 100;
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }
    }
}
