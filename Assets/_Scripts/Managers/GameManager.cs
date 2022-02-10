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

    int customerIndex = 0;

    //[System.NonSerialized]
    //public bool carDriving = true;
    CarController carController;

    bool isPlayingDialog = false;

    [System.NonSerialized]
    public bool inRide;

    [System.NonSerialized]
    public bool 
        displayPickup,
        displayDropoff;

    [HideInInspector]
    public TaskType currentTaskType;

    int totalScore;

    float potentialRideScore;

    bool scoreTicking;

    int rideCount;

    bool[] customerMemories = new bool[] { true, true, true };

    #region References

    [Header("References")]

    public UIManager uiManager;

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

    public CustomClasses.QueryEvent carCollisionEvent;

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
        carController = GameObject.FindGameObjectWithTag("CarDriving").GetComponent<CarController>();
    }

    void Start()
    {
        ratingCountdown.UpdateRating(5f, 0f);

        radialTimer.UpdateRadialTimer();

        scoreText.text = "$0";

        dialogRenderer.EraseDialog();

        foreach (GameObject g in taskTutorialObjects)
        {
            if (g != null)
                g.SetActive(false);
        }

        foreach (GameObject g in taskTutorialObjectsOther)
        {
            if (g != null)
                g.SetActive(false);
        }

        tasksCompleted = new bool[taskTutorialObjects.Length];

        dialogRenderer.DialogCharPrintedEvent.AddListener(PlayDialogSound);

        audioManager.Play("CrowChirping");
    }

    float dialogBoxProgress = 0f;

    // Update
    void Update()
    {
        if (currentCustomer == null || currentPickup == null || currentDropoff == null)
        {
            StartNewCustomer();
        }

        var temp = Random.value;

        //Cursor.visible = uiManager.tutorialLockEnabled || uiManager.pauseLockEnabled;
        //Cursor.lockState = uiManager.tutorialLockEnabled || uiManager.pauseLockEnabled ? CursorLockMode.Confined : CursorLockMode.Locked;

        Cursor.visible = uiManager.pauseLockEnabled;
        Cursor.lockState = uiManager.pauseLockEnabled ? CursorLockMode.Confined : CursorLockMode.Locked;

        dialogBoxProgress = Mathf.MoveTowards(dialogBoxProgress, isPlayingDialog ? 1f : 0f, Time.deltaTime * 4f);

        dialogRenderer.transform.parent.localPosition = Vector3.down * (1f - Mathf.Pow(dialogBoxProgress, 2f)) * 5f;
    }

    void StartNewCustomer(bool skipStart = false)
    {
        rideCount++;

        currentPickup = pickupPool[customerIndex];

        currentDropoff = dropoffPool[customerIndex];

        currentCustomer = customerPool[customerIndex];

        StartCoroutine(CustomerCoroutine(skipStart));

        customerIndex++;
        if (customerIndex >= customerPool.Length || customerIndex >= pickupPool.Length || customerIndex >= dropoffPool.Length)
        {
            customerIndex = 0;
        }
        
        //Mathf.Min(customerIndex + 1, customerPool.Length - 1, pickupPool.Length - 1, dropoffPool.Length - 1);
    }

    IEnumerator CustomerCoroutine(bool skipStart)
    {
        int lastRideCount = rideCount;

        if (!skipStart)
        {
            // Innitialization
            customer = Instantiate(customerPrefab, currentPickup.transform.position, currentPickup.transform.rotation).GetComponent<CustomerController>();

            customer.Initialize(currentCustomer.identity, currentCustomer.hasKids);

            customer.UpdateAnimations(-1);

            scoreTicking = false;

            displayPickup = true;
            displayDropoff = false;

            // Pickup Customer
            currentPickup.trigger.EnableTrigger();

            yield return CustomClasses.WaitUntilEvent(currentPickup.trigger.triggerEvent);

            displayPickup = false;
            displayDropoff = false;

            carController.Braking(2.5f, 0, true);
            //carDriving = false;

            yield return new WaitForSeconds(1f);

            Vector3 customerStartPos = customer.transform.position;
            Quaternion customerStartRot = customer.transform.rotation;

            customer.UpdateAnimations(0);

            for (float t = 0f; t < 1f; t += Time.deltaTime * 0.5f)
            {
                yield return null;

                customer.transform.position = Vector3.Lerp(customerStartPos, customerSitGoal.position, t) + Vector3.up * (1f - Mathf.Pow(t * 2f - 1f, 2f)) * 5f;

                customer.transform.rotation = Quaternion.LerpUnclamped(customerStartRot, customerSitGoal.rotation, t) * Quaternion.AngleAxis(t * 360f * 2f, Vector3.up);
            }

            customer.transform.position = customerSitGoal.position;
            customer.transform.rotation = customerSitGoal.rotation;

            customer.transform.parent = customerSitGoal;

            rideStartEvent.Invoke();

            potentialRideScore = 5f;

            ratingCountdown.UpdateRating(potentialRideScore, 0f);

            radialTimer.UpdateRadialTimer(1f);

            inRide = true;

            StartCoroutine(ScoreUpdateLoopCoroutine());

            yield return new WaitForSeconds(1f);
        }

        if (customerMemories[(int)currentCustomer.identity]) // Implement check for if customer was satisfied during your last ride with them
        {
            PlayDialog(currentCustomer.startRideDialogHappy, DialogAnimationType.Happy);
        }
        else
        {
            PlayDialog(currentCustomer.startRideDialogAngry, DialogAnimationType.Angry);
        }

        //carDriving = true;

        displayPickup = false;
        displayDropoff = true;

        currentDropoff.trigger.EnableTrigger();

        // Handle Tasks
        scoreTicking = true;

        if (isDebugingTasks)
        {
            yield return TaskLoopCoroutine_Debug();
        }
        else
        {
            yield return TaskLoopCoroutine();
        }

        if (lastRideCount != rideCount)
        {
            yield break;
        }

        scoreTicking = false;

        radialTimer.UpdateRadialTimerGoal(false);

        foreach (GameObject g in taskTutorialObjects)
        {
            if (g != null)
                g.SetActive(false);
        }

        foreach (GameObject g in taskTutorialObjectsOther)
        {
            if (g != null)
                g.SetActive(false);
        }

        // Dropoff Customer
        carController.Braking(3.0f, 0, true);
        //carDriving = false;

        displayPickup = false;
        displayDropoff = false;

        if (potentialRideScore >= 2f)
        {
            PlayDialog(currentCustomer.endRideDialogHappy, DialogAnimationType.Happy);

            customerMemories[(int)currentCustomer.identity] = true;
        }
        else
        {
            PlayDialog(currentCustomer.endRideDialogAngry, DialogAnimationType.Angry);

            customerMemories[(int)currentCustomer.identity] = false;
        }

        inRide = false;

        yield return new WaitForSeconds(3f);

        customer.UpdateAnimations(-1);

        for (float t = 1f; t > 0f; t -= Time.deltaTime * 0.5f)
        {
            yield return null;

            customer.transform.position = Vector3.Lerp(currentDropoff.transform.position, customerSitGoal.position, t) + Vector3.up * (1f - Mathf.Pow(t * 2f - 1f, 2f)) * 5f;

            customer.transform.rotation = Quaternion.LerpUnclamped(currentDropoff.transform.rotation, customerSitGoal.rotation, t) * Quaternion.AngleAxis(t * 360f, Vector3.up);
        }

        rideEndEvent.Invoke();

        customer.transform.position = currentDropoff.transform.position;
        customer.transform.rotation = currentDropoff.transform.rotation;

        var lastPos = currentDropoff.transform.position;
        var lastRot = currentDropoff.transform.rotation;

        var lastCustomer = customer;

        customer.transform.parent = null;

        //carDriving = true;

        customer = null;

        currentCustomer = null;

        StartNewCustomer();

        yield return new WaitForSeconds(3f);

        for (float t = 0f; t < 1f; t += Time.deltaTime * 0.5f)
        {
            yield return null;

            lastCustomer.transform.position = Vector3.Lerp(lastPos, lastPos - Vector3.up * 5f, t);

            lastCustomer.transform.rotation = lastRot * Quaternion.AngleAxis(t * 360f * 5f, Vector3.up);
        }
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
            if (currentCustomer.skipBeforeEnd)
            {
                if (Vector3.Distance(GameManager.instance.car.position.RemoveY(), currentDropoff.transform.position.RemoveY()) < 50f)
                {
                    currentTask?.EndTask(this);

                    currentTask = null;
                    currentTaskType = TaskType.Null;

                    radialTimer.UpdateRadialTimerGoal(false);

                    currentDropoff.trigger.DisableTrigger();

                    if ((int)currentTaskType - 2 < taskTutorialObjects.Length && (int)currentTaskType >= 2)
                        if (taskTutorialObjects[(int)currentTaskType - 2] != null)
                            taskTutorialObjects[(int)currentTaskType - 2]?.SetActive(false);

                    if ((int)currentTaskType - 2 < taskTutorialObjectsOther.Length && (int)currentTaskType >= 2)
                        if (taskTutorialObjectsOther[(int)currentTaskType - 2] != null)
                            taskTutorialObjectsOther[(int)currentTaskType - 2]?.SetActive(false);

                    StartNewCustomer(true);

                    yield break;
                }
            }
            else
            {
                if (currentDropoff.trigger.queryEvent.Query())
                {
                    currentTask?.EndTask(this);

                    currentTask = null;
                    currentTaskType = TaskType.Null;

                    break;
                }
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

                    if (taskIndex >= tasksPool.Count)
                    {
                        taskIndex = Random.Range(0, tasksPool.Count);
                    }

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
                                    PlayDialog(currentCustomerTask.mainTaskPrompt, false);
                                    break;

                                case PromptType.Secondary:
                                    PlayDialog(currentCustomerTask.secondaryTaskPrompt, false);
                                    break;

                                case PromptType.Tertiary:
                                    PlayDialog(currentCustomerTask.tertiaryTaskPrompt, false);
                                    break;
                            }

                            taskStartEvent.Invoke();

                            Debug.Log(currentTask);

                            radialTimer.UpdateRadialTimerGoal(true);

                            if (!tasksCompleted[(int)currentTaskType - 2])
                            {
                                if ((int)currentTaskType - 2 < taskTutorialObjects.Length && (int)currentTaskType >= 2) 
                                    if (taskTutorialObjects[(int)currentTaskType - 2] != null)
                                        taskTutorialObjects[(int)currentTaskType - 2].SetActive(true);

                                if ((int)currentTaskType - 2 < taskTutorialObjectsOther.Length && (int)currentTaskType >= 2)
                                    if (taskTutorialObjectsOther[(int)currentTaskType - 2] != null)
                                        taskTutorialObjectsOther[(int)currentTaskType - 2].SetActive(true);
                            }

                            break;
                        }
                    }
                }
            }

            taskIntervalTime -= Time.deltaTime;

            if (currentTask != null)
            {
                currentTask.UpdateTask(this);

                radialTimer.UpdateRadialTimer(taskTimer / taskTimerTotal);

                if (currentTask.completedTaskEvent.Query())
                {
                    currentTask.EndTask(this);

                    if ((int)currentTaskType - 2 < taskTutorialObjects.Length && (int)currentTaskType >= 2)
                        if (taskTutorialObjects[(int)currentTaskType - 2] != null)
                            taskTutorialObjects[(int)currentTaskType - 2]?.SetActive(false);

                    if ((int)currentTaskType - 2 < taskTutorialObjectsOther.Length && (int)currentTaskType >= 2)
                        if (taskTutorialObjectsOther[(int)currentTaskType - 2] != null)
                            taskTutorialObjectsOther[(int)currentTaskType - 2]?.SetActive(false);

                    tasksCompleted[(int)currentTaskType - 2] = true;

                    currentTask = null;
                    currentTaskType = TaskType.Null;

                    taskIntervalTime = Random.Range(currentCustomer.taskTimeMin, currentCustomer.taskTimeMax);

                    PlayDialog(currentCustomerTask.taskCompletionResponse, DialogAnimationType.Happy);

                    taskEndEvent.Invoke();
                    taskCompletionEvent.Invoke();

                    radialTimer.UpdateRadialTimerGoal(false);

                    audioManager.Play("CompleteTask");
                }
                else if (currentTask.failedTaskEvent.Query() || taskTimer <= 0f)
                {
                    currentTask.EndTask(this);

                    if ((int)currentTaskType - 2 < taskTutorialObjects.Length && (int)currentTaskType >= 2)
                        if (taskTutorialObjects[(int)currentTaskType - 2] != null)
                            taskTutorialObjects[(int)currentTaskType - 2].SetActive(false);

                    if ((int)currentTaskType - 2 < taskTutorialObjectsOther.Length && (int)currentTaskType >= 2)
                        if (taskTutorialObjectsOther[(int)currentTaskType - 2] != null)
                            taskTutorialObjectsOther[(int)currentTaskType - 2].SetActive(false);

                    currentTask = null;
                    currentTaskType = TaskType.Null;

                    taskIntervalTime = Random.Range(currentCustomer.taskTimeMin, currentCustomer.taskTimeMax);

                    PlayDialog(currentCustomerTask.taskFailureResponse, DialogAnimationType.Angry);

                    taskEndEvent.Invoke();
                    taskFailureEvent.Invoke();

                    radialTimer.UpdateRadialTimerGoal(false);

                    ScoreLoseStar();

                    audioManager.Play("FailingTask");
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

                        dialogIntervalTime = Random.Range(3f, Mathf.Min(maxInterval, 9f));

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

            if (carCollisionEvent.Query(Time.frameCount, true))
            {
                currentTask?.EndTask(this);

                if ((int)currentTaskType - 2 < taskTutorialObjects.Length && (int)currentTaskType >= 2)
                    if (taskTutorialObjects[(int)currentTaskType - 2] != null)
                        taskTutorialObjects[(int)currentTaskType - 2]?.SetActive(false);

                if ((int)currentTaskType - 2 < taskTutorialObjectsOther.Length && (int)currentTaskType >= 2)
                    if (taskTutorialObjectsOther[(int)currentTaskType - 2] != null)
                        taskTutorialObjectsOther[(int)currentTaskType - 2]?.SetActive(false);

                string crashDialog = currentCustomer.crashDialog[Random.Range(0, currentCustomer.crashDialog.Length)];

                if (currentTask != null)
                {
                    taskIntervalTime = Random.Range(currentCustomer.taskTimeMin, currentCustomer.taskTimeMax);
                }
                else
                {
                    taskIntervalTime = Mathf.Max(taskIntervalTime, EstimateReadTime(crashDialog, dialogRenderer)) + 3f;
                }

                currentTask = null;
                currentTaskType = TaskType.Null;

                PlayDialog(crashDialog, DialogAnimationType.Angry);

                radialTimer.UpdateRadialTimerGoal(false);

                ScoreLoseStar();
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

                        if (!tasksCompleted[(int)currentTaskType - 2])
                        {
                            if ((int)currentTaskType - 2 < taskTutorialObjects.Length)
                                if (taskTutorialObjects[(int)currentTaskType - 2] != null)
                                    taskTutorialObjects[(int)currentTaskType - 2].SetActive(true);

                            if ((int)currentTaskType - 2 < taskTutorialObjectsOther.Length)
                                if (taskTutorialObjectsOther[(int)currentTaskType - 2] != null)
                                    taskTutorialObjectsOther[(int)currentTaskType - 2].SetActive(true);
                        }
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

                    if ((int)currentTaskType - 2 < taskTutorialObjects.Length)
                        if (taskTutorialObjects[(int)currentTaskType - 2] != null)
                            taskTutorialObjects[(int)currentTaskType - 2]?.SetActive(false);

                    if ((int)currentTaskType - 2 < taskTutorialObjectsOther.Length)
                        if (taskTutorialObjectsOther[(int)currentTaskType - 2] != null)
                            taskTutorialObjectsOther[(int)currentTaskType - 2]?.SetActive(false);

                    tasksCompleted[(int)currentTaskType - 2] = true;

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

                    if ((int)currentTaskType - 2 < taskTutorialObjects.Length)
                        if (taskTutorialObjects[(int)currentTaskType - 2] != null)
                            taskTutorialObjects[(int)currentTaskType - 2]?.SetActive(false);

                    if ((int)currentTaskType - 2 < taskTutorialObjectsOther.Length)
                        if (taskTutorialObjectsOther[(int)currentTaskType - 2] != null)
                            taskTutorialObjectsOther[(int)currentTaskType - 2]?.SetActive(false);

                    currentTask = null;
                    currentTaskType = TaskType.Null;

                    taskIntervalTime = 3f;

                    PlayDialog("Task Failed", DialogAnimationType.Angry);

                    taskEndEvent.Invoke();
                    taskFailureEvent.Invoke();

                    ScoreLoseStar();
                }
            }

            radialTimer.UpdateRadialTimer(taskTimer / taskTimerTotal);

            yield return null;
        }
    }

    void PlayDialog(string dialog, DialogAnimationType animationType = DialogAnimationType.Neutral, bool canExpire = true)
    {
        if (dialog == "")
            return;

        dialogRenderer.StartDialog(dialog);
        isPlayingDialog = true;

        customer?.UpdateAnimations((int)animationType);

        StopCoroutine("PlayDialogCoroutine");

        if (canExpire)
            StartCoroutine("PlayDialogCoroutine", dialog);
    }

    void PlayDialog(string dialog, bool canExpire)
    {
        dialogRenderer.StartDialog(dialog);
        isPlayingDialog = true;

        StopCoroutine("PlayDialogCoroutine");

        if (canExpire)
            StartCoroutine("PlayDialogCoroutine", dialog);
    }

    void PlayDialog(string dialog)
    {
        dialogRenderer.StartDialog(dialog);
        isPlayingDialog = true;

        StopCoroutine("PlayDialogCoroutine");

        StartCoroutine("PlayDialogCoroutine", dialog);
    }

    AudioSource currentSourcePrim;
    AudioSource currentSourceSec;

    List<int> intPool = new List<int>();

    int lastPoolPicked;

    public void PlayDialogSound()
    {
        if (intPool.Count == 0)
        {
            intPool.AddRange(new int[] { 0, 1, 2, 3 });

            intPool.Remove(lastPoolPicked);
        }

        bool canPlayPrimary = currentSourcePrim == null;
        if (!canPlayPrimary)
        {
            canPlayPrimary = !currentSourcePrim.isPlaying;
        }

        var random = intPool[Random.Range(0, intPool.Count)];

        if (canPlayPrimary)
        {
            switch (random)
            {
                case 0:
                    currentSourcePrim = audioManager.PlayWithReturn("Dialog 1");
                    break;
                case 1:
                    currentSourcePrim = audioManager.PlayWithReturn("Dialog 2");
                    break;
                case 2:
                    currentSourcePrim = audioManager.PlayWithReturn("Dialog 3");
                    break;
                case 3:
                    currentSourcePrim = audioManager.PlayWithReturn("Dialog 4");
                    break;
            }

            lastPoolPicked = random;

            return;
        }

        ////////

        bool canPlaySecondary = currentSourceSec == null;
        if (!canPlaySecondary)
        {
            canPlaySecondary = !currentSourceSec.isPlaying;
        }

        if (canPlaySecondary)
        {
            switch (random)
            {
                case 0:
                    currentSourceSec = audioManager.PlayWithReturn("Dialog 1");
                    break;
                case 1:
                    currentSourceSec = audioManager.PlayWithReturn("Dialog 2");
                    break;
                case 2:
                    currentSourceSec = audioManager.PlayWithReturn("Dialog 3");
                    break;
                case 3:
                    currentSourceSec = audioManager.PlayWithReturn("Dialog 4");
                    break;
            }

            lastPoolPicked = random;
        }
    }

    IEnumerator PlayDialogCoroutine(string dialog)
    {
        yield return new WaitForSeconds(EstimateReadTime(dialog, dialogRenderer));

        dialogRenderer.EraseDialog();
        isPlayingDialog = false;

        if (customer != null)
        {
            customer.UpdateAnimations(0);
        }
    }

    IEnumerator ScoreUpdateLoopCoroutine()
    {
        float progress = 0f;

        while (inRide)
        {
            yield return null;

            progress = Mathf.Clamp01(progress + Time.deltaTime / 0.5f);

            var lastPotentialRideScore = potentialRideScore;

            if (scoreTicking && currentCustomer != null)
            {
                potentialRideScore = Mathf.Max(0f, potentialRideScore - Time.deltaTime / currentCustomer.starTimeLimit);
            }

            ratingCountdown.UpdateRating(potentialRideScore, progress);

            radialTimer.UpdateRadialTimer();
        }

        int lastScore = totalScore;
        totalScore += Mathf.CeilToInt(potentialRideScore) * 10;

        AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        for (float f = 0; f < 2f; f += Time.deltaTime)
        {
            potentialRideScore = Mathf.Min(potentialRideScore + Time.deltaTime, Mathf.Ceil(potentialRideScore));

            ratingCountdown.UpdateRating(potentialRideScore, progress);

            radialTimer.UpdateRadialTimer();

            scoreText.transform.localScale = Vector3.one * (1f + animationCurve.Evaluate(Mathf.Clamp01(f - 1f)) * 1.5f);

            yield return null;
        }

        if (potentialRideScore > 0f)
        {
            for (float f = 0; f < 1f; f += Time.deltaTime / potentialRideScore)
            {
                float fScaled = 1f - Mathf.Pow(1f - f, 2f);

                scoreText.text = "$" + Mathf.FloorToInt(Mathf.Lerp(lastScore, totalScore, fScaled)).ToString();

                ratingCountdown.UpdateRating(potentialRideScore * (1f - fScaled), false);

                radialTimer.UpdateRadialTimer();

                yield return null;
            }
        }
        else
        {
            for (float f = 0; f < 1f; f += Time.deltaTime)
            {
                ratingCountdown.UpdateRating(0f, false);

                radialTimer.UpdateRadialTimer();

                yield return null;
            }
        }

        scoreText.text = "$" + totalScore.ToString();

        for (float f = 0; f < 1f; f += Time.deltaTime)
        {
            ratingCountdown.UpdateRating(0f, false);

            radialTimer.UpdateRadialTimer();

            yield return null;
        }

        while (progress < 2f)
        {
            progress = Mathf.Min(progress + Time.deltaTime / 0.5f, 2f);

            ratingCountdown.UpdateRating(0f, progress, false);

            radialTimer.UpdateRadialTimer();

            scoreText.transform.localScale = Vector3.one * (2.5f - animationCurve.Evaluate(Mathf.Clamp01(progress - 1f)) * 1.5f);

            yield return null;
        }

        scoreText.transform.localScale = Vector3.one;
    }

    void ScoreLoseStar()
    {
        //potentialRideScore = Mathf.Floor(potentialRideScore) == potentialRideScore ? potentialRideScore - 1f : Mathf.Floor(potentialRideScore);

        potentialRideScore = Mathf.Max(0f, potentialRideScore - 1);

        ratingCountdown.UpdateRating(potentialRideScore);
    }

    #region UI
    [Header("UI References"), SerializeField]
    _Scripts.UI.RatingCountdown ratingCountdown;
    
    [SerializeField]
    RadialTimer radialTimer;

    [SerializeField]
    TMPro.TextMeshProUGUI scoreText;

    [SerializeField]
    GameObject[] 
        taskTutorialObjects, 
        taskTutorialObjectsOther;

    bool[] tasksCompleted;

    void StopDialog()
    {
        StopCoroutine("PlayDialogCoroutine");

        dialogRenderer.StopDialog();

        isPlayingDialog = false;
    }

    #endregion UI

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
        return (text.Length + 5f) / render.textPrintSpeed * 1.8f + 3f;
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
