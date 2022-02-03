using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public CustomerObject[] customerPool;

    public CustomerPickup[] pickupPool;

    public CustomerDropoff[] dropoffPool;

    [System.NonSerialized]
    public CustomerObject currentCustomer;

    [System.NonSerialized]
    public CustomerPickup currentPickup;

    [System.NonSerialized]
    public CustomerDropoff currentDropoff;

    [System.NonSerialized]
    public CustomerController customer;

    int customerIndex;

    [System.NonSerialized]
    public bool carDriving = true;

    bool isPlayingDialog = false;

    [HideInInspector]
    public TaskType currentTaskType;

    #region References

    [Header("References")]

    public Transform car;

    public AudioManager audioManager;

    [SerializeField]
    DialogRenderer dialogRenderer;

    [SerializeField]
    Transform customerSitGoal;

    public TaskReferences taskReferences;

    [Header("Prefabs")]

    [SerializeField]
    GameObject customerPrefab;

    #endregion References

    #region Callbacks

    [Header("Callbacks")]
    public UnityEvent rideStartEvent;
    public UnityEvent rideEndEvent;

    public UnityEvent taskStartEvent;
    public UnityEvent taskEndEvent;

    public UnityEvent taskCompletionEvent;
    public UnityEvent taskFailureEvent;

    #endregion Callbacks

    [SerializeField]
    bool isDebugingTasks;

    // Start
    void Awake()
    {
        if (instance != null && instance )
        {
            Destroy(gameObject);
        }

        instance = this;
    }

    // Update
    void Update()
    {
        if (currentCustomer == null || currentPickup == null || currentDropoff == null)
        {
            StartNewCustomer();
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void StartNewCustomer()
    {
        currentPickup = pickupPool[WeightedDistanceRandomization(pickupPool, car)];

        currentDropoff = dropoffPool[WeightedDistanceRandomization(dropoffPool, currentPickup.transform)];

        currentCustomer = customerPool[customerIndex];

        customerIndex++;
        if (customerIndex >= customerPool.Length)
        {
            customerIndex = customerPool.Length - 1;
        }

        StartCoroutine("CustomerCoroutine");
    }

    IEnumerator CustomerCoroutine()
    {
        // Create Customer
        customer = Instantiate(customerPrefab, currentPickup.transform.position, currentPickup.transform.rotation).GetComponent<CustomerController>();

        // Pickup Customer
        currentPickup.trigger.EnableTrigger();

        yield return CustomClasses.WaitUntilEvent(currentPickup.trigger.triggerEvent);

        carDriving = false;

        yield return new WaitForSeconds(1f);

        Vector3 customerStartPos = customer.transform.position;
        Quaternion customerStartRot = customer.transform.rotation;

        for (float t = 0f; t < 1f; t += Time.deltaTime * 1.2f)
        {
            yield return null;

            customer.transform.position = Vector3.Lerp(customerStartPos, customerSitGoal.position, t) + Vector3.up * (1f - Mathf.Pow(t * 2f - 1f, 2f)) * 5f;

            customer.transform.rotation = Quaternion.LerpUnclamped(customerStartRot, customerSitGoal.rotation, t) 
                * Quaternion.AngleAxis(t * 360f * 3f, Quaternion.Euler(Vector3.up * 90f) * (customerSitGoal.position - customerStartPos).RemoveY().normalized)
                * Quaternion.AngleAxis(t * 360f, Vector3.up);
        }

        customer.transform.position = customerSitGoal.position;
        customer.transform.rotation = customerSitGoal.rotation;

        customer.transform.parent = customerSitGoal;

        rideStartEvent.Invoke();

        yield return new WaitForSeconds(1f);

        if (true) // Implement check for if customer was satisfied during your last ride with them
        {
            PlayDialog(currentCustomer.startRideDialogHappy, DialogAnimationType.Happy);
        }
        else
        {
            PlayDialog(currentCustomer.startRideDialogAngry, DialogAnimationType.Angry);
        }

        carDriving = true;

        currentDropoff.trigger.EnableTrigger();

        // Handle Tasks
        if (isDebugingTasks)
        {
            yield return TaskLoopCoroutine_Debug();
        }
        else
        {
            yield return TaskLoopCoroutine();
        }

        // Dropoff Customer
        carDriving = false;

        if (true) // Implement check for if customer was satisfied
        {
            PlayDialog(currentCustomer.endRideDialogHappy, DialogAnimationType.Happy);
        }
        else
        {
            PlayDialog(currentCustomer.endRideDialogAngry, DialogAnimationType.Angry);
        }

        yield return new WaitForSeconds(Mathf.Max(EstimateReadTime(currentCustomer.endRideDialogHappy, dialogRenderer), 3f));
        
        for (float t = 1f; t > 0f; t -= Time.deltaTime * 1.2f)
        {
            yield return null;

            customer.transform.position = Vector3.Lerp(currentDropoff.transform.position, customerSitGoal.position, t) + Vector3.up * (1f - Mathf.Pow(t * 2f - 1f, 2f)) * 5f;

            customer.transform.rotation = Quaternion.LerpUnclamped(currentDropoff.transform.rotation, customerSitGoal.rotation, t)
                * Quaternion.AngleAxis(t * 360f * 3f, Quaternion.Euler(Vector3.up * 90f) * (currentDropoff.transform.position - customerSitGoal.position).RemoveY().normalized)
                * Quaternion.AngleAxis(t * 360f, Vector3.up);
        }

        rideEndEvent.Invoke();

        customer.transform.position = currentDropoff.transform.position;
        customer.transform.rotation = currentDropoff.transform.rotation;

        customer.transform.parent = null;

        carDriving = true;

        customer = null;

        currentCustomer = null;

        StartNewCustomer();
    }

    IEnumerator TaskLoopCoroutine()
    {
        List<CustomerTask> tasksPool = new List<CustomerTask>();

        CustomerTask currentCustomerTask = null;
        Task currentTask = null;

        float taskTimerTotal = 0;
        float taskTimer = 0;

        float taskIntervalTime = Random.Range(9f, 15f);

        float dialogIntervalTime = Random.Range(8f, 12f);

        int nextDialogIndex = 0;

        bool canPlayDialog = false;

        while (true)
        {
            if (currentDropoff.trigger.queryEvent.Query())
            {
                currentTask?.EndTask(this);

                break;
            }

            taskTimer -= Time.deltaTime;

            if (currentTask == null && currentCustomer.taskPool.Length > 0 && taskIntervalTime <= 0f)
            {
                if (tasksPool.Count == 0)
                {
                    tasksPool.AddRange(currentCustomer.taskPool);
                }

                var taskIndex = Random.Range(0, tasksPool.Count -1);

                while (tasksPool.Count > 0)
                {
                    currentCustomerTask = tasksPool[taskIndex];
                    tasksPool.RemoveAt(taskIndex);

                    currentTask = Task.CreateTask(currentCustomerTask.taskType);
                    currentTaskType = currentCustomerTask.taskType;

                    if (currentTask != null)
                    {
                        if (!currentTask.CheckValid(this))
                        {
                            currentTask = null;
                            currentTaskType = TaskType.Null;
                        }
                        else
                        {
                            taskTimerTotal = currentCustomerTask.timeLimit;
                            taskTimer = taskTimerTotal;

                            switch (currentTask.StartTask(this))
                            {
                                default:
                                    PlayDialog(currentCustomerTask.mainTaskPrompt);
                                    break;

                                case PromptType.Secondary:
                                    PlayDialog(currentCustomerTask.secondaryTaskPrompt);
                                    break;

                                case PromptType.Tertiary:
                                    PlayDialog(currentCustomerTask.tertiaryTaskPrompt);
                                    break;
                            }

                            taskStartEvent.Invoke();

                            break;
                        }
                    }
                }
            }

            taskIntervalTime -= Time.deltaTime;

            if (currentTask != null)
            {
                currentTask.UpdateTask(this);
                Debug.Log(currentTask);

                if (currentTask.completedTaskEvent.Query())
                {
                    currentTask.EndTask(this);

                    currentTask = null;
                    currentTaskType = TaskType.Null;

                    taskIntervalTime = Random.Range(currentCustomer.taskTimeMin, currentCustomer.taskTimeMax);

                    PlayDialog(currentCustomerTask.taskCompletionResponse, DialogAnimationType.Happy);

                    taskEndEvent.Invoke();
                    taskCompletionEvent.Invoke();
                }
                else if (currentTask.failedTaskEvent.Query() || taskTimer <= 0f)
                {
                    currentTask.EndTask(this);

                    currentTask = null;
                    currentTaskType = TaskType.Null;

                    taskIntervalTime = Random.Range(currentCustomer.taskTimeMin, currentCustomer.taskTimeMax);

                    PlayDialog(currentCustomerTask.taskFailureResponse, DialogAnimationType.Angry);

                    taskEndEvent.Invoke();
                    taskFailureEvent.Invoke();
                }

                canPlayDialog = false;
            }
            else
            {
                if (nextDialogIndex < currentCustomer.generalDialog.Length)
                {
                    if (!canPlayDialog && !isPlayingDialog)
                    {
                        var maxInterval = taskIntervalTime - EstimateReadTime(currentCustomer.generalDialog[nextDialogIndex], dialogRenderer) - 2f;

                        dialogIntervalTime = Random.Range(3f, Mathf.Clamp(maxInterval, 3f, 9f));

                        if (dialogIntervalTime < 3f)
                        {
                            canPlayDialog = false;
                        }
                        else
                        {
                            canPlayDialog = true;
                        }
                    }

                    if (canPlayDialog)
                    {
                        dialogIntervalTime -= Time.deltaTime;

                        if (dialogIntervalTime <= 0f && !isPlayingDialog)
                        {
                            PlayDialog(currentCustomer.generalDialog[nextDialogIndex]);

                            nextDialogIndex++;

                            canPlayDialog = false;
                        }
                    }
                }
            }

            yield return null;
        }
    }

    IEnumerator TaskLoopCoroutine_Debug()
    {
        float taskTimer = 0;
        float taskTimerTotal = 0;

        float taskIntervalTime = 5f;

        Task currentTask = null;

        while (true)
        {
            if (currentDropoff.trigger.queryEvent.Query())
            {
                currentTask?.EndTask(this);

                break;
            }

            int numberPressed = GetNumberKeyPressed();
            if (numberPressed > -1)
            {
                currentTask?.EndTask(this);

                currentTask = null;
                currentTaskType = TaskType.Null;

                currentTaskType = (TaskType)(numberPressed + 1);

                currentTask = Task.CreateTask(currentTaskType);

                if (currentTask != null)
                {
                    if (!currentTask.CheckValid(this))
                    {
                        currentTask = null;
                        currentTaskType = TaskType.Null;
                    }
                    else
                    {
                        taskTimerTotal = 20f;
                        taskTimer = taskTimerTotal;

                        PlayDialog(GetDebugDialog(currentTask.StartTask(this), currentTaskType));

                        taskStartEvent.Invoke();
                    }
                }
            }

            taskTimer -= Time.deltaTime;

            taskIntervalTime -= Time.deltaTime;

            if (currentTask != null)
            {
                currentTask.UpdateTask(this);

                if (currentTask.completedTaskEvent.Query())
                {
                    currentTask.EndTask(this);

                    currentTask = null;
                    currentTaskType = TaskType.Null;

                    taskIntervalTime = 3f;

                    PlayDialog("Task Completed", DialogAnimationType.Happy);

                    taskEndEvent.Invoke();
                    taskCompletionEvent.Invoke();
                }
                else if (currentTask.failedTaskEvent.Query() || taskTimer <= 0f)
                {
                    currentTask.EndTask(this);

                    currentTask = null;
                    currentTaskType = TaskType.Null;

                    taskIntervalTime = 3f;

                    PlayDialog("Task Failed", DialogAnimationType.Angry);

                    taskEndEvent.Invoke();
                    taskFailureEvent.Invoke();
                }
            }

            yield return null;
        }
    }

    void PlayDialog(string dialog, DialogAnimationType animationType = DialogAnimationType.Neutral)
    {
        dialogRenderer.StartDialog(dialog);
        isPlayingDialog = true;

        StopCoroutine("PlayDialogCoroutine");

        StartCoroutine("PlayDialogCoroutine", dialog);
    }

    IEnumerator PlayDialogCoroutine(string dialog)
    {
        yield return new WaitForSeconds(EstimateReadTime(dialog, dialogRenderer));

        dialogRenderer.EraseDialog();
        isPlayingDialog = false;
    }

    void StopDialog()
    {
        StopCoroutine("PlayDialogCoroutine");

        dialogRenderer.StopDialog();

        isPlayingDialog = false;
    }

    public static int WeightedDistanceRandomization(MonoBehaviour[] objectArray, Transform distanceToTransform)
    {
        float[] weights = new float[objectArray.Length];
        var total = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = Vector3.Distance(objectArray[i].transform.position, distanceToTransform.position);

            total += weights[i];
        }

        var value = Random.Range(0f, total);

        for (int i = 0; i < weights.Length; i++)
        {
            value -= weights[i];

            if (value <= 0f)
            {
                return i;
            }
        }

        return 0;
    }

    public static float EstimateReadTime(string text, DialogRenderer render)
    {
        return (text.Length + 5f) / render.textPrintSpeed * 1.5f + 2f;
    }

    // OnValidate
    void OnValidate()
    {
        name = "GameManager";
    }

    // Reset
    void Reset()
    {
        name = "GameManager";
    }

    // OnGUI
    void OnGUI()
    {
        if (isDebugingTasks)
        {
            string taskList = "Tasks:";

            for (int i = 0; i < 9; i++)
            {
                taskList += "\n" + i.ToString() + ". " + (TaskType)(i + 1);
            }

            GUI.TextArea(new Rect(10, Screen.height - 180, 120, 170), taskList, 200);
        }
    }

    public static GameManager instance;

    public enum DialogAnimationType
    {
        Neutral,
        Happy,
        Angry,
    }

    int GetNumberKeyPressed()
    {
        if (Input.inputString != "")
        {
            int number;
            bool is_a_number = int.TryParse(Input.inputString, out number);
            if (is_a_number && number >= 0 && number < 10)
            {
                return number;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }

    string GetDebugDialog(PromptType promptType, TaskType taskType)
    {
        string s = "[c0]" + taskType.ToString() + "[c] Started.";

        switch (taskType)
        {
            case TaskType.OpenWindow:
                s += " [c0]";

                if (promptType == PromptType.Main)
                {
                    s += "Open";
                }
                else
                {
                    s += "Close";
                }

                s += "[c] the window.";
                break;

            case TaskType.ChangeAc:
                s += " Set the AC to [c0]";

                if (currentCustomer.volumePreference != SettingPreference.None)
                {
                    s += currentCustomer.tempPreference.ToString();
                }
                else
                {
                    s += (SettingPreference)(promptType + 1);
                }

                s += "[c].";
                break;

            case TaskType.ChangeVolume:
                s += " Set the volume to [c0]";

                if (currentCustomer.volumePreference != SettingPreference.None)
                {
                    s += currentCustomer.volumePreference.ToString();
                }
                else
                {
                    s += (SettingPreference)(promptType + 1);
                }

                s += "[c].";
                break;

            case TaskType.ChangeMusic:
                s += " Set the music to [c0]";

                if (promptType == PromptType.Main)
                {
                    s += currentCustomer.musicPreference.ToString();
                }
                else
                {
                    s += currentCustomer.musicPreferenceOther.ToString();
                }

                s += "[c].";
                break;
        }

        return s;
    }
}
