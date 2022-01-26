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
    public CustomerTask currentTask;

    [System.NonSerialized]
    public CustomerController customer;

    int customerIndex;

    Task taskInstance;

    [System.NonSerialized]
    public bool carDriving = true;

    #region References

    [Header("Car References")]

    public Transform car;
    public Transform steeringWheel;

    [Header("General References")]

    [SerializeField]
    Transform customerSitGoal;

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
        customer = Instantiate(customerPrefab, currentPickup.transform.position, currentPickup.transform.rotation).GetComponent<CustomerController>();

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

        carDriving = true;

        currentDropoff.trigger.EnableTrigger();

        yield return CustomClasses.WaitUntilEvent(currentDropoff.trigger.triggerEvent);

        carDriving = false;

        yield return new WaitForSeconds(1f);

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
