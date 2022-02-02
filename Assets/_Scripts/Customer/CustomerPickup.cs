using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPickup : MonoBehaviour
{
    public Trigger trigger;

    [SerializeField]
    Canvas canvas;

    void Start()
    {
        trigger.triggerEvent.AddListener(OnCarEnterTrigger);
    }

    void Update()
    {
        canvas.enabled = GameManager.instance.currentPickup == this;
    }

    void OnCarEnterTrigger()
    {
        //
    }
}
