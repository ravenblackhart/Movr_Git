using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.ScriptableVariables;
using ScriptableEvents;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

                    if (SceneManager.GetActiveScene().buildIndex == 1)
                    {
                        go.AddComponent<RadialTimer>(); 
                    }
                }
            }

            return instance; 
        }
    }

    #region Inspector

    [Header("Panels")] 
    [SerializeField] private Canvas pauseMenu; 
    
    [Header("Display Fields")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI passengerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image ratingDisplay; 

    [Header("Data")] 
    [SerializeField] private IntVariable score;
    [SerializeField] private CustomerObject customer;
    [SerializeField] private FloatVariable startingRating; 

    [Header("Events")] 
    [SerializeField] private ScriptableEventInt scoreUpdate; 

    #endregion

    #region Other Declarations

    private float rating = 5f; 
    

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
        
    }
    

    private void Start()
    {
        pauseMenu.enabled = false; 
        SetScoreText(score.IntValue.ToString());
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PauseGame();
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // Test Scripts
            if (Keyboard.current.dKey.isPressed) PassengerPickUp();
        }
        
    }

    #endregion
    
    
    public void PauseGame()
    {
        if (pauseMenu.enabled == false)
        {
            pauseMenu.enabled = true; 
            Time.timeScale = 0;
        }

        else
        {
            pauseMenu.enabled = false; 
            Time.timeScale = 1; 
        }
        
    }

    public void Restart()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PassengerPickUp()
    {
        StartCoroutine(RatingCountDown()); 
    }

    public void TaskCountDown()
    {
        StartCoroutine(RadialTimer()); 
    }
    
    private void SetScoreText(string text)
    {
        scoreText.text = text; 
    }

    private void SetDialogueText(string text)
    {
        dialogueText.text = text; 
    }

    private void SetPassengerNameText(string text)
    {
        passengerNameText.text = text; 
    }

    private IEnumerator RatingCountDown()
    {
        while (rating > 0f)
        {
            ratingDisplay.fillAmount = rating * 0.2f;
            rating -= (Time.deltaTime * 0.5f);
            yield return null; 
        }
        
    }

    private IEnumerator RadialTimer()
    {
        yield return null; 
    }

    #region Protoype
    
    public void TestSteer()
    {
        SceneManager.LoadScene("CarMoveTesting");
    }

    public void TestFPV()
    {
        SceneManager.LoadScene("PrototypeScene");
    }

    public void CombinedTest()
    {
        SceneManager.LoadScene("PrototypeTest");
    }


    #endregion

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }

}