using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.ScriptableVariables;
using JetBrains.Annotations;
using ScriptableEvents;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

[System.Serializable]
public class MyEvent : UnityEvent<GameObject>
{
}
public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "UIManager";
                    instance = go.AddComponent<UIManager>();

                    if (SceneManager.GetActiveScene().buildIndex == 1 && go.GetComponent<RadialTimer>()== null)
                    {
                        go.AddComponent<RadialTimer>(); 
                    }
                }
            }

            return instance; 
        }
    }

    #region Inspector

    [Header("Displays")] 
    [SerializeField] [CanBeNull] private Canvas settingsPanel;
    [SerializeField] [CanBeNull] private Canvas creditsPanel;
    [SerializeField] [CanBeNull] private Canvas readyPanel;
    [SerializeField] [CanBeNull] private Canvas tutorialCanvas;
    [SerializeField] [CanBeNull] private Canvas pauseMenu;
    [SerializeField] [CanBeNull] private Canvas gameOverMenu;

    [Header("Display Fields - Menu")] 
    [SerializeField] [CanBeNull] private Toggle tutorialState; 
    [SerializeField] [CanBeNull] private TextMeshProUGUI tsLabel;
    
    [Header("Display Fields - Game")]
    [SerializeField] [CanBeNull] private TextMeshProUGUI scoreText;
    [SerializeField] [CanBeNull] private TextMeshProUGUI gameOverScoreText; 

    [Header("Data")] 
    [SerializeField] [CanBeNull] private IntVariable score;
    [SerializeField] [CanBeNull] private CustomerObject customer;
    [SerializeField] [CanBeNull] private FloatVariable startingRating; 

    [Header("Events")] 
    [SerializeField] private ScriptableEventInt scoreUpdate; 
    
    

    #endregion

    #region Other Declarations

    //Gameplay Data
    private float rating = 5f; 
    
    //Settings
    private bool tutorialOn = true; 
    

    
    
    
    

    #endregion
    #region Defaults
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this; 
        }

        if (PlayerPrefs.GetString("TutorialState") == "true") tutorialOn = true; 
        else if (PlayerPrefs.GetString("TutorialState") == "false") tutorialOn = false; 
    }
    

    private void Start()
    {
        
        
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            settingsPanel.enabled = false;
            creditsPanel.enabled = false;
            tsLabel.text = tutorialOn ? "Tutorial On" : "Tutorial Off";
            tutorialState.isOn = tutorialOn; 
        }
        
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            pauseMenu.enabled = false;
            gameOverMenu.enabled = false; 
            SetScoreText(score.IntValue.ToString());

            if (tutorialOn)
            {
                readyPanel.enabled = true;
                tutorialCanvas.enabled = true; 
            }
            
            else if (!tutorialOn)
            {
                readyPanel.enabled = false;
                tutorialCanvas.enabled = false; 
            }
        }
    }

    private void Update()
    {
        if ((Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed) && readyPanel.enabled == true)
        {
            readyPanel.enabled = false;
            Time.timeScale = 1f; 
        } 
        if (Keyboard.current.escapeKey.wasPressedThisFrame) PauseGame();

    }

    #endregion

    #region Main Menu UI Functions

    public void OpenPanel(Canvas panel)
    {
        panel.enabled = true;
        
    }

    public void ClosePanel(Canvas panel)
    {
        panel.enabled = false; 

    }
    

    #endregion
    
    public void PauseGame()
    {
        if (pauseMenu.enabled == false)
        {
            pauseMenu.enabled = true; 
            Time.timeScale = 0f;
        }

        else
        {
            Time.timeScale = 1f; 
            pauseMenu.enabled = false; 
            
        }
        
    }

    public void GameOver()
    {
        gameOverScoreText.text = score.ToString(); 
        gameOverMenu.enabled = true;
        Time.timeScale = 0f;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    
    private void SetScoreText(string text)
    {
        scoreText.text = text; 
    } 

    public void TutorialState()
    {
        tutorialOn = tutorialState.isOn? true : false;
        tsLabel.text = tutorialOn ? "Tutorial On" : "Tutorial Off"; 
        PlayerPrefs.SetString("TutorialState", tutorialOn ? "true" : "false" );
    }



    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    

}