using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPickup : MonoBehaviour
{
    public Trigger trigger;

    [SerializeField]
    GameObject testCylinder;

    [SerializeField]
    Canvas canvas;

    void Start()
    {
        trigger.triggerEvent.AddListener(OnCarEnterTrigger);
    }

    void Update()
    {
        canvas.enabled = GameManager.instance.currentPickup == this;

        if (testCylinder != null)
            testCylinder.SetActive(GameManager.instance.currentPickup == this);
    }

    void OnCarEnterTrigger()
    {
        //
    }
}
