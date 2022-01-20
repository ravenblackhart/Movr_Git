using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void TestSteer()
    {
        SceneManager.LoadScene("CarMoveTesting"); 
    }
    
    public void TestFPV()
    {
        SceneManager.LoadScene("PrototypeScene"); 
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("0_MainMenu"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}