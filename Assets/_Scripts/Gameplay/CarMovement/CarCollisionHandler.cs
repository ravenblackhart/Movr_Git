using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionHandler : MonoBehaviour
{

    private int[] _randomBinaryNumbers = new int[10] { 0, 1, 0, 1, 1, 0, 1, 1, 0, 0};

    private int _index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 0)
        {
            if (_randomBinaryNumbers[_index] == 0)
            {
            //Debug.LogWarning("se dig för: " + other.gameObject.name);
                AudioManager.Instance.Play("HardImpact");
            }
            else
            {
                //Debug.LogWarning("se dig för: " + other.gameObject.name);
                AudioManager.Instance.Play("MediumImpact");
            }
            if (_index < _randomBinaryNumbers.Length)
            {
                _index++;
            }
            else
            {
                _index = 0;
            }
            
        }
    }
}
