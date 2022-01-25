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

    [TextArea]
    public string startRideDialog;

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

    [System.Serializable]
    public class CustomerTask
    {
        public TaskType taskType;

        public float timeLimit = 20f;

        [TextArea]
        public string
            mainTaskPrompt,
            alternateTaskPrompt,

            taskCompletionResponse,
            taskFailureResponse;
    }
}