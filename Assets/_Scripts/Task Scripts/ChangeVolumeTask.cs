using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVolumeTask : Task
{
    float leverGoal;

    float completionProgress;

    SettingPreference settingGoal;

    public override PromptType StartTask(GameManager gameManager)
    {
        if (gameManager.currentCustomer.volumePreference == SettingPreference.None)
        {
            switch (settingGoal)
            {
                default:
                    return PromptType.Main;

                case SettingPreference.Mid:
                    return PromptType.Secondary;

                case SettingPreference.Low:
                    return PromptType.Tertiary;
            }
        }
        else
        {
            return PromptType.Main;
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

    public override bool CheckValid(GameManager gameManager)
    {
        settingGoal = gameManager.currentCustomer.volumePreference;

        if (settingGoal == SettingPreference.None)
        {
            settingGoal = (SettingPreference)Random.Range(1, 3);

            SetLeverGoal();

            if (LeverInRange(gameManager))
            {
                settingGoal = (SettingPreference)(settingGoal + 1);

                SetLeverGoal();
            }
        }
        else
        {
            SetLeverGoal();
        }

        return !LeverInRange(gameManager);
    }

    public bool LeverInRange(GameManager gameManager)
    {
        return Mathf.Abs(gameManager.taskReferences.volumeLever.LeverValue - leverGoal) <= 0.16f;
    }

    public void SetLeverGoal()
    {
        switch (settingGoal)
        {
            default:
                leverGoal = 0.5f;
                break;

            case SettingPreference.High:
                leverGoal = 0.87f;
                break;

            case SettingPreference.Mid:
                leverGoal = 0.5f;
                break;

            case SettingPreference.Low:
                leverGoal = 0.15f;
                break;
        }
    }
}
