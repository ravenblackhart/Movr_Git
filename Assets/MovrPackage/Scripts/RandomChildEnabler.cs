using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChildEnabler : MonoBehaviour
{
    // Start
    void Start()
    {
        int randomValue = Random.Range(0, transform.childCount);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(randomValue == i);
        }
    }
}
