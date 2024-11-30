using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playAudio(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    public void StartGame()
    {
        playAudio(clickSound);
        SceneManager.LoadScene("Scenes/SampleScene");
    }
    
    public void QuitGame()
    {
        playAudio(clickSound);
        Application.Quit();
    }
}
