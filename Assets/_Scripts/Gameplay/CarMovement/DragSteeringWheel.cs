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

    private enum SteerDirection { Straight, Left, Right}
    private SteerDirection _steering = SteerDirection.Straight;
    private bool _currentlySteering = false;
    [SerializeField]
    private float _steerOffset = 1f;
    [SerializeField]
    private float _resetSpeed = 30f;

    private CarController _carControl;
    private CameraSwitcher _cameraSwitcher;

    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    private PlayerInput _playerInput;
    private InputAction _mouseClick;
    private Vector2 _mouseDelta;
    private Vector2 _prevDelta;

    [SerializeField]
    private float _rotateSpeed = 20;
    private float _zAngle = 0;

    private bool _readyToReset = false;

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _cameraSwitcher = FindObjectOfType<CameraSwitcher>();
        _steeringWheel = GameObject.FindGameObjectWithTag(_steeringWheelTag);
        _mainCamera = Camera.main;
        _carControl = GetComponent<CarController>();

        Cursor.lockState = CursorLockMode.Confined;
        
        _mouseClick = _playerInput.actions["PrimaryAction"];
    }

    void Update()
    {
        // Find mouse movement
        _mouseDelta = _playerInput.actions["MouseLook"].ReadValue<Vector2>().normalized;
        
        //Resets Steering Wheel
        if (!_currentlySteering && _steering != SteerDirection.Straight && _readyToReset)
        {
            _zAngle = ResetSteeringWheel(_steering, _zAngle);
            _steeringWheel.transform.localEulerAngles = new Vector3(0, 0, _zAngle);
        }
        else if (_steering == SteerDirection.Straight)
        {
            _readyToReset = false;
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
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }
    }


    private IEnumerator DragUpdate(GameObject steerWheel)
    {
        _cameraSwitcher.ToggleLock();
        
        while (_mouseClick.ReadValue<float>() != 0)
        {
            if (_mouseDelta.x != 0)
            {
                _prevDelta = _mouseDelta;
            }
            
            //Car turning
            // _carControl.turn = _prevDelta.x * _rotateSpeed * Time.deltaTime;
            _carControl.turn = Mathf.Lerp(_carControl.turn, _prevDelta.x, Time.deltaTime);
            
            //Steering Wheel Rotate
            steerWheel.transform.localEulerAngles = 
                new Vector3(0, 0, _zAngle -= _prevDelta.x * _rotateSpeed * Time.deltaTime);

            //Determine steer direction
            if (_prevDelta.x < 0)
            {
                _steering = SteerDirection.Left;
            }
            else if (_prevDelta.x > 0)
            {
                _steering = SteerDirection.Right;
            }
            else
            {
                _steering = SteerDirection.Straight;
            }

            yield return _waitForFixedUpdate;
        }
        _cameraSwitcher.ToggleLock();
        StartCoroutine(WaitForReset());
        _currentlySteering = false;
    }

    private IEnumerator WaitForReset()
    {
        yield return new WaitForSeconds(1.0f);
        _readyToReset = true;
    }

    private float TurningAngle(Vector3 direction, float zAngle)
    {
        if (zAngle != 0)
        {
           
        }
        float radian = Mathf.Atan2(direction.x, direction.y);
        if (direction.z < 0)
        {
            //radian = Mathf.Atan2(direction.x, direction.y);
            //_carControl.turn = radian / Mathf.PI * 2;
            //radian = Mathf.Atan2(direction.x, -direction.y); // yx, xz yz zx zy, n�stan ok: x,-y
        }
        else
        {
            //_carControl.turn = radian / Mathf.PI * 2;
        }

        //_carControl.turn = radian / Mathf.PI * 2;// * zAngle;// * (1 / (zAngle + 0.05f));
        //Debug.Log(_carControl.turn);
        //return Mathf.Atan2(direction.x, direction.y) * (180 / Mathf.PI);
        float angle = radian * (180 / Mathf.PI);
        if (direction.z < 0)
        {
            //angle *= -1;
            //angle -= 180;
        }
        if (angle > -_steerOffset)
        {
            _steering = SteerDirection.Left;
        }
        else if (angle < _steerOffset)
        {
            _steering = SteerDirection.Right;
            //Debug.Log(angle);
            //return 180;
        }
        return angle;


        // dont work: return Vector3.Angle(go.transform.position, direction);
        //if (direction.y < 0)
        //{
        //    return 360 -direction.x * 360;
        //}
        //else
        //{
        //    return direction.x * 360;
        //}
    }

    

    private float ResetSteeringWheel(SteerDirection steerDirection, float angle)
    {
        //if (steerDirection == SteerDirection.Left)
        //{
        //    angle += _resetSpeed * Time.deltaTime;
        //}
        //else if (steerDirection == SteerDirection.Right)
        //{
        //    angle -= _resetSpeed * Time.deltaTime;
        //}
        if (angle < 0)
        {
            angle += _resetSpeed * Time.deltaTime;
        }
        else if (angle > 0)
        {
            angle -= _resetSpeed * Time.deltaTime;
        }
        if (angle > -_steerOffset && angle < _steerOffset)
        {
            _steering = SteerDirection.Straight;
            _carControl.turn = 0;
            return 0;
        }
        //_carControl.turn = (Mathf.PI / 180) * angle;
        return angle;
    }
}
