using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent triggerEvent;

    public CustomClasses.QueryEvent queryEvent;

    Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();

        collider.isTrigger = true;

        DisableTrigger();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.instance.car)
        {
            triggerEvent.Invoke();

            queryEvent.Invoke();

            DisableTrigger();
        }
    }

    public void EnableTrigger()
    {
        collider.enabled = true;
    }

    public void DisableTrigger()
    {
        collider.enabled = false;
    }
}
