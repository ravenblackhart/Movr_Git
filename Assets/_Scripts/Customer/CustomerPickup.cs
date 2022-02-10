using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPickup : MonoBehaviour
{
    public Trigger trigger;

    [SerializeField]
    GameObject testCylinder;

    void Start()
    {
        trigger.triggerEvent.AddListener(OnCarEnterTrigger);
    }

    void Update()
    {
        if (testCylinder != null)
            testCylinder.SetActive(GameManager.instance.currentPickup == this && GameManager.instance.displayPickup);
    }

    void OnCarEnterTrigger()
    {
        //
    }
}
