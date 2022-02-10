using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionHandler : MonoBehaviour
{

    private int[] _randomBinaryNumbers = new int[10] { 0, 1, 0, 1, 1, 0, 1, 1, 0, 0};

    private int _index = 0;

    private bool landed = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForLanding());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // If collides with object with the Default layer
        if (other.gameObject.layer == 0 && landed)
        {
            GameManager.instance.carCollisionEvent.Invoke(Time.frameCount);

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
            if (_index < _randomBinaryNumbers.Length - 1)
            {
                _index++;
            }
            else
            {
                _index = 0;
            }
            
        }
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitForSeconds(2f);
        landed = true;
    }
}
