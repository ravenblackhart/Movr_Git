using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPickup : MonoBehaviour
{
    public Trigger trigger;

    [SerializeField]
    Canvas canvas;

    GameManager gameManager;

    void Start()
    {
        trigger.triggerEvent.AddListener(OnCarEnterTrigger);

        gameManager = GameManager.instance;
    }

    void Update()
    {
        canvas.enabled = gameManager.currentPickup == this;
    }

    void OnCarEnterTrigger()
    {
        //
    }
}
