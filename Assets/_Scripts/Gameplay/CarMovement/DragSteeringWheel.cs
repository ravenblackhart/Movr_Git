using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CarController))]
public class DragSteeringWheel : MonoBehaviour
{
    [Header("The steering wheel needs a matching tag to make it work!")]
    [SerializeField]
    private string _steeringWheelTag = "SteeringWheel";
    private GameObject _steeringWheel;

    private Camera _mainCamera;

    private enum SteerDirection { Straight, Left, Right }
    private SteerDirection _steering = SteerDirection.Straight;
    private bool _currentlySteering = false;

    private CarController _carController;
    private CameraSwitcher _cameraSwitcher;

    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    private PlayerInput _playerInput;
    private InputAction _mouseClick;
    private Vector2 _mouseDelta;
    private Vector2 _prevDelta;
    [Header("Speed for rotating the steering wheel")]
    [Range(0.1f, 5f)]
    [SerializeField]
    private float _rotateSpeed = 2;
    private float _zAngle = 90;

    [SerializeField]
    private int _maxSteerAngle = 125;

    [SerializeField]
    private float _resetSteerOffset = 3f;
    [Header("Reset speed for the steering wheel")]
    [SerializeField]
    private float _resetSpeed = 30f;

    [Header("Reset speed for steering the car")]
    [SerializeField]
    private float _speedModifierReset = 0.5f;

    [Header("Should match the z-rotation of the steering wheel object")]
    [SerializeField]
    private int _rotationOffset = 90;
    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _cameraSwitcher = FindObjectOfType<CameraSwitcher>();
        _steeringWheel = GameObject.FindGameObjectWithTag(_steeringWheelTag);
        _mainCamera = Camera.main;
        _carController = GetComponent<CarController>();

        Cursor.lockState = CursorLockMode.Confined;
        
        _mouseClick = _playerInput.actions["PrimaryAction"];
    }

    void Update()
    {
        // Find mouse movement
        _mouseDelta = _playerInput.actions["MouseLook"].ReadValue<Vector2>();//.normalized;
        
        // Resets Steering Wheel
        if (!_currentlySteering && _steering != SteerDirection.Straight)
        {
            _zAngle = ResetSteeringWheel(_zAngle);
            _steeringWheel.transform.localEulerAngles = new Vector3(_steeringWheel.transform.localEulerAngles.x,
                _steeringWheel.transform.localEulerAngles.y, _zAngle);
        }
    }

    private void OnEnable()
    {
        _mouseClick.Enable();
        _mouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        _mouseClick.performed -= MousePressed;
        _mouseClick.Disable();
    }

    private void MousePressed(InputAction.CallbackContext context)
    {
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = _mainCamera.ScreenPointToRay(center);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject.tag == _steeringWheelTag)
            {
                _currentlySteering = true;
                StartCoroutine(DragUpdate());
            }
        }
    }

    [SerializeField]
    private float _maxTurn = 5f;

    private IEnumerator DragUpdate()
    {
        _cameraSwitcher.ToggleLock();
        
        while (_mouseClick.ReadValue<float>() != 0)
        {
            if (_mouseDelta.x != 0)
            {
                _prevDelta = _mouseDelta;
            }
            float turn = Mathf.Clamp( _prevDelta.x * _rotateSpeed * Time.deltaTime, -_maxTurn, _maxTurn);
            _zAngle += _prevDelta.x * _rotateSpeed * Time.deltaTime;
            _zAngle = Mathf.Clamp(_zAngle, _rotationOffset - _maxSteerAngle, _rotationOffset + _maxSteerAngle);
            // Determine steer direction
            if (_prevDelta.x < 0)
            {
                _steering = SteerDirection.Left;
            }
            else if (_prevDelta.x > 0)
            {
                _steering = SteerDirection.Right;
            }
            else if (_mouseDelta.x == 0)
            {
                _steering = SteerDirection.Straight;
                _zAngle = _rotationOffset;
                turn = 0;
                //Debug.LogWarning(_steering);
            }
            
            // Car turning
            _carController.turn = Mathf.Lerp(_carController.turn, turn, Time.deltaTime);
            // Rotate Steering Wheel 
            _steeringWheel.transform.localEulerAngles = new Vector3(_steeringWheel.transform.localEulerAngles.x,
                _steeringWheel.transform.localEulerAngles.y, _zAngle);


            yield return _waitForFixedUpdate;
        }
        _cameraSwitcher.ToggleLock();
        _currentlySteering = false;
    }

    private float ResetSteeringWheel(float angle)
    {
        if (angle < _rotationOffset)
        {
            angle += _resetSpeed * Time.deltaTime * Mathf.Abs(_zAngle - _rotationOffset);
            if (_carController.turn > Time.deltaTime * _speedModifierReset)
            {
                _carController.turn -= Time.deltaTime * _speedModifierReset;
            }
            
        }
        else if (angle > _rotationOffset)
        {
            angle -= _resetSpeed * Time.deltaTime * Mathf.Abs(_zAngle - _rotationOffset);
            if (_carController.turn < Time.deltaTime * _speedModifierReset)
            {
                _carController.turn += Time.deltaTime * _speedModifierReset;
            }
        }
        if (angle > _rotationOffset - _resetSteerOffset && angle < _rotationOffset + _resetSteerOffset)
        {
            _steering = SteerDirection.Straight;
            _carController.turn = 0;
            return _rotationOffset;
        }
        return angle;
    }
}
