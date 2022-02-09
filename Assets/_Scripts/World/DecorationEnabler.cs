using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationEnabler : MonoBehaviour
{
    [SerializeField]
    float renderRange = 50f;

    // Update
    void Update()
    {
        bool inRange = (transform.position - GameManager.instance.car.position).RemoveY().magnitude <= renderRange;

        foreach (Transform c in transform)
        {
            c.gameObject.SetActive(inRange);
        }
    }
}
