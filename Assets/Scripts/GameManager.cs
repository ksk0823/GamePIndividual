using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("GameObjects")]
    public PlayerController player;
    
    public GameObject _UIObject;
    public GameObject _FPSObject;

    public GameObject pausePanel;
    public GameObject gamePanel;
    private AudioSource audioSource;
    public AudioClip clickSound;
    
    private int HP = 30;
    private int goal = 30;
    private TextMeshProUGUI _UIText;
    private TextMeshProUGUI _FPSText;
    private float deltaTime = 0.0f;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Duplicated GameManager.");
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {
        _UIText = _UIObject.GetComponent<TextMeshProUGUI>();
        _FPSText = _FPSObject.GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        
        ShowUI();
        ShowFPS();
        
        if (HP <= 0)
        {
            Lose();
        } else if (goal <= 0)
        {
            Win();
        }
    }

    public void ShowUI()
    {
        _UIText.SetText("Goal to win : " + goal +"\nHP : " + HP);
    }

    public void ShowFPS()
    {
        float fps = 1.0f / deltaTime;
        _FPSText.SetText(string.Format("FPS : {0:0.00}", fps));
    }

    public void DecreaseHP(int damage)
    {
        HP -= damage;
    }

    public void DecreaseGoal()
    {
        goal--;
    }

    void Lose()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Scenes/LoseScene");
    }

    void Win()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Scenes/WinScene");
    }

    public void pauseGame()
    {
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playAudio(clickSound);
        Time.timeScale = 0f;
    }

    public void resumeGame()
    {
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playAudio(clickSound);
        Time.timeScale = 1f;
        player.pauseGame = false;
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/StartScene");
    }
    
    public void playAudio(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
