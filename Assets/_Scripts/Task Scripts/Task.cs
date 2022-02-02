using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Task
{
    public TaskType type;

    public Task SetType(TaskType type)
    {
        this.type = type;

        return this;
    }

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
                return null;
            case TaskType.Default:
                return new Task().SetType(taskType);
            case TaskType.ChangeMusic:
                return new ChangeMusicTask().SetType(taskType);
            case TaskType.ChangeVolume:
                return new ChangeVolumeTask().SetType(taskType);
            case TaskType.OpenWindow:
                return new OpenWindowTask().SetType(taskType);
            case TaskType.GiveWater:
                return new GiveWaterTask().SetType(taskType);
            case TaskType.GiveSnackBar:
                return new GiveSnackBarTask().SetType(taskType);
            case TaskType.ChangeAc:
                return new ChangeAcTask().SetType(taskType);
            case TaskType.PlayCatch:
                return new PlayCatchTask().SetType(taskType);
            case TaskType.ChargePhone:
                return new ChargePhoneTask().SetType(taskType);
        }
    }
}

public class TaskParams
{
    public int promptIndex;
}

public enum TaskType
{
    Null,
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