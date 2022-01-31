using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Task
{
    public virtual PromptType StartTask(GameManager gameManager)
    {
        return PromptType.Main;
    }

    public virtual void UpdateTask(GameManager gameManager)
    {
        //
    }

    public virtual void EndTask(GameManager gameManager)
    {
        //
    }

    public virtual bool CheckValid(GameManager gameManager)
    {
        return true;
    }

    public CustomClasses.QueryEvent completedTaskEvent, failedTaskEvent;

    public static Task CreateTask(TaskType taskType)
    {
        switch (taskType)
        {
            default:
                return new Task();
            case TaskType.ChangeMusic:
                return new ChangeMusicTask();
            case TaskType.ChangeVolume:
                return new ChangeVolumeTask();
            case TaskType.OpenWindow:
                return new OpenWindowTask();
            case TaskType.GiveWater:
                return new GiveWaterTask();
            case TaskType.GiveSnackBar:
                return new GiveSnackBarTask();
            case TaskType.ChangeAc:
                return new ChangeAcTask();
            case TaskType.PlayCatch:
                return new PlayCatchTask();
            case TaskType.ChargePhone:
                return new ChargePhoneTask();
        }
    }
}

public class TaskParams
{
    public int promptIndex;
}

public enum TaskType
{
    Default,
    ChangeMusic,
    ChangeVolume,
    OpenWindow,
    GiveWater,
    GiveSnackBar,
    ChangeAc,
    PlayCatch,
    ChargePhone,
}

public enum PromptType
{
    Main,
    Secondary,
    Tertiary,
}