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