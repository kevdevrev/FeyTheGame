using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public AudioMixer audioMixer;

    // Set up build indexes in file -> build settings. This will go to next index when start button is pressed.
    public  void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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


}
