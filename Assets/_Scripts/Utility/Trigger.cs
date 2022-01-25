using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent triggerEvent;

    Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();

        collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == GameManager.instance.car)
        {
            triggerEvent.Invoke();

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
