using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScenes2D : MonoBehaviour
{
    [SerializeField] private string level;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Fey"))
        {
            other.GetComponent<Fey>().SavePlayer();
            SceneManager.LoadScene(level);
        }
    }
}
