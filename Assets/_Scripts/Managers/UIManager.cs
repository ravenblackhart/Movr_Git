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

    [Header("Panels")] 
    [SerializeField] [CanBeNull] private Canvas settingsPanel;
    [SerializeField] [CanBeNull] private Canvas leaderboardPanel; 
    [SerializeField] [CanBeNull] private Canvas creditsPanel;
    [SerializeField] [CanBeNull] private Canvas readyPanel;
    [SerializeField] [CanBeNull] private Canvas pauseMenu;

    [Header("Display Fields - Menu")] 
    [SerializeField] [CanBeNull] private TMP_InputField setUID;
    [SerializeField] [CanBeNull] private TextMeshProUGUI saveMessage;
    [SerializeField] [CanBeNull] private Transform leaderTable;
    
    [Header("Display Fields - Game")]
    [SerializeField] [CanBeNull] private TextMeshProUGUI scoreText;
    [SerializeField] [CanBeNull] private TextMeshProUGUI passengerNameText;
    [SerializeField] [CanBeNull] private TextMeshProUGUI dialogueText;
    [SerializeField] [CanBeNull] private Image ratingDisplay; 

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
    
    //Script Internal
    
    private float posXIn = 0f;
    private float posYIn = 120f;

    private float posXOut = 0f;
    private float posYOut = 1175f;

    private float elapsedAnimDuration = 0;
    private float percentAnim;
    private float timeOffset = 5f;
    private bool gameOn = false;
    
    private Vector2 startPosition;
    private Vector2 targetPosition;

    private bool animatePanel = false;
    private RectTransform animTarget;

    private PlayFabManager playFab;

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
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            settingsPanel.enabled = false;
            leaderboardPanel.enabled = false; 
            creditsPanel.enabled = false;
        }
        
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            pauseMenu.enabled = false; 
            SetScoreText(score.IntValue.ToString());

            readyPanel.enabled = true;
        }
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