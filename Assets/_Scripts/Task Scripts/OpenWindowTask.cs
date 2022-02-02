using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWindowTask : Task
{
    float leverGoal;

    float completionProgress;

    public override PromptType StartTask(GameManager gameManager)
    {
        if (gameManager.taskReferences.windowsLever.LeverValue > 0.5f)
        {
            leverGoal = 0f;

            return PromptType.Main;
        }
        else
        {
            leverGoal = 1f;

            return PromptType.Secondary;
        }
    }

    public override void UpdateTask(GameManager gameManager)
    {
        if (LeverInRange(gameManager))
        {
            completionProgress += Time.deltaTime * 2f;
        }
        else
        {
            completionProgress = 0f;
        }

        if (completionProgress >= 1f)
        {
            completedTaskEvent.Invoke();
        }
    }

    public bool LeverInRange(GameManager gameManager)
    {
        return Mathf.Abs(gameManager.taskReferences.volumeLever.LeverValue - leverGoal) <= 0.2f;
    }
}
