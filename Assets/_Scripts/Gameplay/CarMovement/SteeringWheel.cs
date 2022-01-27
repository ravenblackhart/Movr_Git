using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SteeringWheel : MonoBehaviour
{
    

    private Vector3 _mousePosition;
    private Camera _mainCamera;


    //public Transform tempPos;

    private bool _steering = false;

    private float _wheelAngle = 0f;
    private float _zAngle = 0f;
    private float _lastWheelAngle = 0f;
    private Vector3 center;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        //SteeringActivated();
    }

    // Update is called once per frame
    void Update()
    {
        // OnAim(): var projectedMousePosition = _mainCamera.ScreenToWorldPoint(_mousePosition);

        //var mousePosition = PlayerInput.actions["aim"].ReadValue<Vector2>();
        //var projectedMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        var mousePos = Mouse.current.position.ReadValue();

        //_carControl.turn = _wheelAngle * 0.5f;
        //transform.localEulerAngles += new Vector3(_wheelAngle * 5f, 0, 0);
        transform.localEulerAngles = new Vector3(-10, 0, _zAngle += _wheelAngle);
        //Debug.Log(transform.localRotation);
        // uselt: transform.localRotation = new Quaternion(_zAngle += _wheelAngle * 5f * Time.deltaTime, transform.localRotation.y, transform.localRotation.x, 0);

        // uselt2: transform.Rotate(tempPos.position, _wheelAngle * 50f * Time.deltaTime);
        // usellllttllt 3: transform.Rotate(transform.position, _wheelAngle * 50f * Time.deltaTime);

        if (Keyboard.current.aKey.isPressed && _wheelAngle > -1)
        {
            _wheelAngle -= Time.deltaTime;
        }
        else if (Keyboard.current.dKey.isPressed && _wheelAngle < 1)
        {
            _wheelAngle += Time.deltaTime;
        }
        else if (!Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed)
        {
            _wheelAngle = 0;
        }
    }

    void OnAim(InputAction.CallbackContext context)
    {
        //_mousePosition = context.ReadValue<Vector2>();
    }

    public void RotateSteeringWheel(float direction)
    {
        
    }
}
