using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDropoff : MonoBehaviour
{
    public Trigger trigger;

    [SerializeField]
    GameObject testCylinder;

    void Start()
    {
        trigger.triggerEvent.AddListener(OnCarExitTrigger);
    }

    void Update()
    {
        if (testCylinder != null)
            testCylinder.SetActive(GameManager.instance.currentDropoff == this && GameManager.instance.displayDropoff);
    }

    void OnCarExitTrigger()
    {
        //
    }
}
