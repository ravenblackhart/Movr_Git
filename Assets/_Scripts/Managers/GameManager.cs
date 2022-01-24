using System.Collections;
using System.Collections.Generic;
using _Scripts.ScriptableVariables;
using ScriptableEvents;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Declarations

    [Header("Score")] 
    [SerializeField] private IntVariable score;
    [SerializeField] private ScriptableEventInt onScoreUpdate;
    private int passengerResult; 

    #endregion

    private void ScoreUpdate()
    {
        score.ApplyChange(passengerResult);
        onScoreUpdate.Raise(score.IntValue);
    }
}
