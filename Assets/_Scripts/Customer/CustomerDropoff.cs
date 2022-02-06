using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDropoff : MonoBehaviour
{
    public Trigger trigger;

    [SerializeField]
    GameObject testCylinder;

    [SerializeField]
    Canvas canvas;

    void Start()
    {
        trigger.triggerEvent.AddListener(OnCarExitTrigger);
    }

    void Update()
    {
        canvas.enabled = GameManager.instance.currentDropoff == this;

        if (testCylinder != null)
            testCylinder.SetActive(GameManager.instance.currentDropoff == this);
    }

    void OnCarExitTrigger()
    {
        //
    }
}
