using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Task
{
    public virtual void StartTask(GameManager gameManager)
    {
        //
    }

    public virtual void UpdateTask(GameManager gameManager)
    {
        //
    }

    public virtual void CancelTask(GameManager gameManager)
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
        }
    }
}

public enum TaskType
{
    Default,
    ChangeMusic,
    OpenWindow,
    GiveWater,
    GiveSnack,
    ChangeAc,
}
