using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDropoff : MonoBehaviour
{
    public Trigger trigger;

    GameManager gameManager;

    void Start()
    {
        trigger.triggerEvent.AddListener(OnCarExitTrigger);

        gameManager = GameManager.instance;
    }

    void OnCarExitTrigger()
    {
        //
    }
}
