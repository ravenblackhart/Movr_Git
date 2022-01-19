using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Steering : MonoBehaviour
{
    [Range(0.01f, 5)]
    [SerializeField]
    private float _resetSpeed = 1;
    [Range(0.1f, 3)]
    [SerializeField]
    private float _resetTimer = 1;
    [SerializeField]
    private float _turnSpeed = 0.2f;

    [SerializeField]
    private float _steering = 0;
    [SerializeField]
    private int _steerSpeed = 20;
    private bool _curTurning = false;

    private float _increase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + _steering * Time.deltaTime * _steerSpeed, transform.eulerAngles.z);
        //transform.Rotate(new Vector3(0, _steering * 20, 0), Space.Self);
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (_steering < 0.001f && _steering > -0.001f && !_curTurning)
        {
            Reset();
        }
        else
        {
            _steering = 0;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            StartCoroutine(StartTurning(_turnSpeed));
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            StartCoroutine(StartTurning(-_turnSpeed));
        }
        if (_curTurning && _increase < 0)
        {
            _steering += _increase - Time.deltaTime * _steerSpeed;
        }
        else if (_curTurning && _increase > 0)
        {
            _steering += _increase + Time.deltaTime * _steerSpeed;
        }
    }

    private void Turn(float amount)
    {
        _steering += amount;
    }

    private void Reset()
    {
        if (_steering > 0)
        {
            _steering -= Time.deltaTime * _resetSpeed;
        }
        else
        {
            _steering += Time.deltaTime * _resetSpeed;
        }
    }

    private IEnumerator StartTurning(float amount)
    {
        _curTurning = true;
        _increase = amount;
        _steering += amount;
        yield return new WaitForSeconds(_resetTimer);
        _curTurning = false;
    }
}
