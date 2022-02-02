using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicTask : Task
{
    public override PromptType StartTask(GameManager gameManager)
    {
        //Check for preference in the future

        return PromptType.Main;
    }

    public override void UpdateTask(GameManager gameManager)
    {
        //Check for the right music tracks later
        if (gameManager.taskReferences.cassettePlayer.occupied) {
            completedTaskEvent.Invoke();
        }
    }

    public override void EndTask(GameManager gameManager)
    {
        //
    }

    public override bool CheckValid(GameManager gameManager)
    {
        if (gameManager.taskReferences.cassetteTapes.Count != 0) {
            return true;
        }

        else {
            return false;
        }
    }
}
