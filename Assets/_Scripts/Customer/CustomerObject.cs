using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerObject", menuName = "ScriptableObjects/Customers", order = 1)]
public class CustomerObject : ScriptableObject
{
    public CustomerIdentity identity;

    public bool hasKids;

    public CustomerTask[] taskPool;

    public float starTimeLimit = 40f;

    [Tooltip("The minimum interval between tasks.")]
    public float taskTimeMin;

    [Tooltip("The maximum interval between tasks.")]
    public float taskTimeMax;

    public SettingPreference tempPreference;

    public SettingPreference volumePreference;

    public MusicPreference musicPreference;
    public MusicPreference musicPreferenceOther;

    [TextArea]
    public string startRideDialogHappy;

    [TextArea]
    public string startRideDialogAngry;

    [TextArea]
    public string endRideDialogHappy;

    [TextArea]
    public string endRideDialogAngry;

    [TextArea]
    public string specialEventDialog;

    [TextArea]
    public string[] generalDialog;

    [TextArea]
    public string[] crashDialog;
}

[System.Serializable]
public class CustomerTask
{
    public TaskType taskType;

    public float timeLimit = 20f;

    [TextArea]
    public string
        mainTaskPrompt,
        secondaryTaskPrompt,
        tertiaryTaskPrompt,

        taskCompletionResponse,
        taskFailureResponse,
        taskFailureResponseAlternate;

    [TextArea]
    public string[] specialTaskDialog;
}

public enum MusicPreference
{
    Any,
    Rock,
    Jazz,
    Funk,
    House,
}

public enum SettingPreference
{
    None,
    High,
    Mid,
    Low,
}

public enum CustomerIdentity
{
    Tom,
    Max,
    Wendy,
}