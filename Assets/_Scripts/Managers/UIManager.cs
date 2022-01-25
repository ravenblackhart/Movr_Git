using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.ScriptableVariables;
using ScriptableEvents;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Inspector

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

    private float rating; 
    

    #endregion

    private void Start()
    {
        SetScoreText(score.IntValue.ToString()); 
    }

    public void PauseGame()
    {
        if (Time.timeScale != 0) Time.timeScale = 0;
        else Time.timeScale = 1; 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PassengerPickUp()
    {
        
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

    private Enum RatingCountDown()
    {
        rating -= (Time.deltaTime * 0.2f);
        ratingDisplay.fillAmount = rating * 0.2f; 
        return null; 
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

    public void MainMenu()
    {
        SceneManager.LoadScene("0_MainMenu");
    }
    #endregion


}