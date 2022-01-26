using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPickup : MonoBehaviour
{
    public Trigger trigger;

    GameManager gameManager;

    void Start()
    {
        trigger.triggerEvent.AddListener(OnCarEnterTrigger);

        gameManager = GameManager.instance;
    }

    void OnCarEnterTrigger()
    {
        //
    }
}
