using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region References

    [Header("References")]

    public Transform car;

    [SerializeField]
    DialogRenderer dialogRenderer;

    [SerializeField]
    Transform customerSitGoal;

    public TaskReferences taskReferences;

    [Header("Prefabs")]

    [SerializeField]
    GameObject customerPrefab;

    #endregion References

    // Start
    void Start()
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

        yield return new WaitForSeconds(1f);

        PlayDialog(currentCustomer.startRideDialogHappy);

        carDriving = true;

        currentDropoff.trigger.EnableTrigger();

        // Handle Tasks
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

                var taskIndex = Random.Range(0, tasksPool.Count);

                while (tasksPool.Count > 0)
                {
                    currentCustomerTask = tasksPool[taskIndex];
                    tasksPool.RemoveAt(taskIndex);

                    currentTask = Task.CreateTask(currentCustomerTask.taskType);

                    if (!currentTask.CheckValid(this))
                    {
                        currentTask = null;
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

                        break;
                    }
                }
            }

            taskIntervalTime -= Time.deltaTime;

            if (currentTask != null)
            {
                currentTask.UpdateTask(this);

                if (currentTask.completedTaskEvent.Query())
                {
                    currentTask.EndTask(this);

                    currentTask = null;

                    taskIntervalTime = Random.Range(currentCustomer.taskTimeMin, currentCustomer.taskTimeMax);

                    PlayDialog(currentCustomerTask.taskCompletionResponse);

                    // Add event completion callback
                }
                else if (currentTask.failedTaskEvent.Query() || taskTimer <= 0f)
                {
                    currentTask.EndTask(this);

                    currentTask = null;

                    taskIntervalTime = Random.Range(currentCustomer.taskTimeMin, currentCustomer.taskTimeMax);

                    PlayDialog(currentCustomerTask.taskFailureResponse);

                    // Add event failure callback
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

        StopDialog();

        // Dropoff Customer
        carDriving = false;

        PlayDialog(currentCustomer.endRideDialogHappy);

        yield return new WaitForSeconds(Mathf.Max(EstimateReadTime(currentCustomer.endRideDialogHappy, dialogRenderer), 3f));
        
        for (float t = 1f; t > 0f; t -= Time.deltaTime * 1.2f)
        {
            yield return null;

            customer.transform.position = Vector3.Lerp(currentDropoff.transform.position, customerSitGoal.position, t) + Vector3.up * (1f - Mathf.Pow(t * 2f - 1f, 2f)) * 5f;

            customer.transform.rotation = Quaternion.LerpUnclamped(currentDropoff.transform.rotation, customerSitGoal.rotation, t)
                * Quaternion.AngleAxis(t * 360f * 3f, Quaternion.Euler(Vector3.up * 90f) * (currentDropoff.transform.position - customerSitGoal.position).RemoveY().normalized)
                * Quaternion.AngleAxis(t * 360f, Vector3.up);
        }

        customer.transform.position = currentDropoff.transform.position;
        customer.transform.rotation = currentDropoff.transform.rotation;

        customer.transform.parent = null;

        carDriving = true;

        customer = null;

        currentCustomer = null;

        StartNewCustomer();
    }

    void PlayDialog(string dialog)
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

    public static GameManager instance;
}
