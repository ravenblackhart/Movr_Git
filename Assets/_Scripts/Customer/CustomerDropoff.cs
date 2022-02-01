using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDropoff : MonoBehaviour
{
    public Trigger trigger;

    [SerializeField]
    Canvas canvas;

    GameManager gameManager;

    void Start()
    {
        trigger.triggerEvent.AddListener(OnCarExitTrigger);

        gameManager = GameManager.instance;
    }

    void Update()
    {
        canvas.enabled = gameManager.currentDropoff == this;
    }

    void OnCarExitTrigger()
    {
        //
    }
}
