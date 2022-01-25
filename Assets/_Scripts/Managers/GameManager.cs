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
    public bool carDriving = true;

    #region References

    [Header("Interactable References")]

    public Transform car;
    public Transform steeringWheel;

    [Header("System References")]

    public Transform customerSitGoal;

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
        //
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
