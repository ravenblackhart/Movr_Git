using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPickup : MonoBehaviour
{
    [SerializeField] Trigger trigger;

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

    IEnumerator Fade()
    {
        gameManager.carDriving = false;

        yield return new WaitForSeconds(1f);

        for (float f = 0f; f < 1f; f += Time.deltaTime)
        {
            //

            yield return null;
        }

        gameManager.carDriving = true;
    }
}
