using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerObject", menuName = "ScriptableObjects/Customers", order = 1)]
public class CustomerObject : ScriptableObject
{
    public Material bodyMaterial;

    public bool
        hasHat,
        hasGlasses;

    public CustomerTask[] taskPool;

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
    Jazz,
    Tech,
    Pop,
    Rock,
}

public enum SettingPreference
{
    None,
    High,
    Mid,
    Low,
}