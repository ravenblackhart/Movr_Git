using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    
    [SerializeField]
    private float _curSpeed = 5f;
    [SerializeField]
    private float _topSpeed = 10f;
    [SerializeField]
    private float _reverseSpeed = 3f;

    // readonlys:
    [SerializeField]
    private float _defAccSpeed = 2;
    [SerializeField]
    private float _defRetardSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Cruising:
        _rigidbody.velocity += transform.forward * _curSpeed * Time.deltaTime;
            //new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (Keyboard.current.wKey.isPressed)
        {
            Accelerate(_topSpeed);
        }
        else if (Keyboard.current.rKey.isPressed)
        {

            Reverse(_reverseSpeed);
        }
        else if (Keyboard.current.sKey.isPressed)
        {

            Retardation(0, _defRetardSpeed);
        }
    }

    private void Accelerate(float topSpeed)
    {
        do
        {
            //Debug.Log("acc");
            _curSpeed += Time.deltaTime * _defAccSpeed;
        } while (_curSpeed < topSpeed);
    }

    private void Reverse(float topSpeed)
    {
        if (_curSpeed > 0)
        {
            Retardation(0, 10f);
        }
        do
        {
            //Debug.Log("ret");
            _curSpeed -= Time.deltaTime * _defAccSpeed;
        } while (_curSpeed > -topSpeed);
    }

    private void Retardation(float targetSpeed, float retardSpeed)
    {
        do
        {
            //Debug.Log("stop");
            _curSpeed -= Time.deltaTime * retardSpeed;
        } while (_curSpeed > targetSpeed);
        _curSpeed = 0;
    }
}
