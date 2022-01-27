using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.ScriptableVariables;
using ScriptableEvents;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

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

    [Header("Panels")] 
    [SerializeField] private Canvas readyPanel;
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

        readyPanel.enabled = true;
       
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.isPressed && readyPanel.enabled == true) readyPanel.enabled = false; 
        if (Keyboard.current.escapeKey.wasPressedThisFrame) PauseGame();

        
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // Test Scripts
            if (Keyboard.current.dKey.isPressed) PassengerPickUp(rating);
        }
        
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
            pauseMenu.enabled = false; 
            Time.timeScale = 1f; 
        }
        
    }

    public void PlayGame()
    {
        Debug.Log("Let's Play");
        SceneManager.LoadScene(1); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PassengerPickUp(float baseRating)
    {
        StartCoroutine(RatingCountDown(baseRating)); 
    }

    public void TaskCountDown()
    {
        StartCoroutine(RadialTimer()); 
    }
    
    private void SetScoreText(string text)
    {
        scoreText.text = text; 
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text; 
    }

    public void SetPassengerName(string text)
    {
        passengerNameText.text = text; 
    }

    private IEnumerator RatingCountDown(float baseRating)
    {
        rating = baseRating; 
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
        SceneManager.LoadScene(1);
    }


    #endregion

    public void MainMenu()
    {
        Debug.Log("heading to Main Menu");
        SceneManager.LoadScene(0);
    }
    

}