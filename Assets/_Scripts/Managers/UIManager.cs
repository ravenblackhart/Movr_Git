using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Inspector

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI passengerRatingText; 

    #endregion

    private void Start()
    {
        SetScoreText(); 
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
    
    private void SetScoreText()
    {
        
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