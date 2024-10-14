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
    
    public GameObject _UIObject;
    
    private int HP = 30;
    private int goal = 30;
    private TextMeshProUGUI _UIText;

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
    }

    // Update is called once per frame
    void Update()
    {
        ShowUI();
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
        _UIText.SetText("Goal to win: " + goal +"\nHP: " + HP);
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
        SceneManager.LoadScene("Scenes/LoseScene");
    }

    void Win()
    {
        SceneManager.LoadScene("Scenes/WinScene");
    }
}
