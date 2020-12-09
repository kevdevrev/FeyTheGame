using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class MainMenuController : MonoBehaviour
{
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject optionsMenu;
    [SerializeField] public AudioMixer audioMixer;
    private Fey_Animation _fey_animation;

    private void Start()
    {
        _fey_animation = transform.GetChild(0).GetComponent<Fey_Animation>();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           // _fey_animation.animateAttack();
        }
    }

    // Set up build indexes in file -> build settings. This will go to next index when start button is pressed.
    public  void PlayGame()
    {
        //_fey_animation.animateAttack();
        StartCoroutine(StartLoadTime());
    }

    public void QuitGame()
    {
        // Debug.Log("Exiting game..");
        Application.Quit();
    }

    public void options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    // Go back to main menu. Disable settings menu
    public void back()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void SetVoume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    IEnumerator StartLoadTime()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
