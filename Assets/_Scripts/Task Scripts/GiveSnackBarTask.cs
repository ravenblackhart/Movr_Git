using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveSnackBarTask : Task
{
    public override PromptType StartTask(GameManager gameManager)
    {
        return PromptType.Main;
    }

    public override void UpdateTask(GameManager gameManager)
    {
        foreach (SnackBar bar in gameManager.taskReferences.snackBars) {
            if (bar._touchingCustomer) {
                Debug.Log("The snackbar touching him is " + bar);
                completedTaskEvent.Invoke();
            }
        }
    }

    public override void EndTask(GameManager gameManager)
    {
        
    }

    public override bool CheckValid(GameManager gameManager)
    {
        return true;
    }
}
