using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    [SerializeField] [CanBeNull] private int score;

    [System.NonSerialized]
    public bool tutorialLockEnabled = true;

    [System.NonSerialized]
    public bool pauseLockEnabled;

    #endregion

    #region Other Declarations

    //Settings
    public Color EnabledColor = new Color32(56, 56, 56, 255); 
    public Color DisabledColor = new Color32(120, 120, 120, 255);

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

        if (!PlayerPrefs.HasKey("TutorialState"))
        {
            PlayerPrefs.SetInt("TutorialState", 1);
        }

        if (PlayerPrefs.GetInt("TutorialState") == 1)
        {
            tutorialOn = true;
        } 
        
        else if (PlayerPrefs.GetInt("TutorialState") == 0)
        {
            tutorialOn = false;
        }
        
    }
    

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            settingsPanel.enabled = false;
            creditsPanel.enabled = false;

            if (tutorialOn == false)
            {
                tutorialState.isOn = false; 
                tsLabel.text = "Tutorial Off";
                tsLabel.color = DisabledColor;
            }
            
            else if (tutorialOn == true)
            {
                tutorialState.isOn = true;
                tsLabel.text = "Tutorial On";
                tsLabel.color = EnabledColor;
            }
            
        }
        
        else if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            pauseMenu.enabled = false;
            gameOverMenu.enabled = false; 
            SetScoreText(score.ToString());

            if (tutorialOn == true)
            {
                readyPanel.enabled = true;
                tutorialCanvas.enabled = true;
                Time.timeScale = 0f; 
               
            }
            
            else if (tutorialOn == false)
            {
                readyPanel.enabled = false;
                tutorialCanvas.enabled = false;
                Time.timeScale = 1f; 
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if ((Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed) && readyPanel.enabled == true)
            {
                tutorialLockEnabled = false;

                readyPanel.enabled = false;
                Time.timeScale = 1f; 
            } 
            if (Keyboard.current.escapeKey.wasPressedThisFrame) PauseGame();
        }
        
        

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
        var cameraSwitcher = FindObjectOfType<CameraSwitcher>();
        
        if (pauseMenu.enabled == false)
        {
            pauseMenu.enabled = true; 
            Time.timeScale = 0f;
            cameraSwitcher.SetToLockedCamera();

        }

        else
        {
            Time.timeScale = 1f;
            pauseMenu.enabled = false;
            cameraSwitcher.SetToFrontCamera();
            
        }

        pauseLockEnabled = pauseMenu.enabled;
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
        //scoreText.text = text; 
    } 

    public void TutorialState()
    {
        tsLabel.text = tutorialState.isOn ? "Tutorial On" : "Tutorial Off";

        if (tutorialState.isOn == true)
        {
            PlayerPrefs.SetInt("TutorialState",1);
            tsLabel.color = EnabledColor;
        }
        
        else if (tutorialState.isOn == false)
        {
            PlayerPrefs.SetInt("TutorialState", 0);
            tsLabel.color = DisabledColor;
        }
    }



    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    

}