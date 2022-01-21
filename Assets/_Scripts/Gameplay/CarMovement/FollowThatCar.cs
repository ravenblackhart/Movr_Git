using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowThatCar : MonoBehaviour
{
    public GameObject car;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(car.transform.position.x, 3.17f, car.transform.position.z - 6.31f);
    }
}
