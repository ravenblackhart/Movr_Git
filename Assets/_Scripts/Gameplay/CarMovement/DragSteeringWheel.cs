using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CarController))]
public class DragSteeringWheel : MonoBehaviour
{
    [SerializeField]
    private InputAction _mouseClick;
    [Header("The steering wheel needs a matching tag to make it work!")]
    [SerializeField]
    private string _steeringWheelTag = "SteeringWheel";
    private GameObject _steeringWheel;

    private Camera _mainCamera;

    private enum SteerDirection { Straight, Left, Right}
    private SteerDirection _steering = SteerDirection.Straight;
    private bool _currentlySteering = false;
    private float _steerOffset = 0.5f;
    private float _resetSpeed = 50f;

    private CarController _carControl;

    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    private void Awake()
    {
        _steeringWheel = GameObject.FindGameObjectWithTag(_steeringWheelTag);
        _mainCamera = Camera.main;
        _carControl = GetComponent<CarController>();
    }

    void Update()
    {
        if (!_currentlySteering && _steering != SteerDirection.Straight)
        {
            _zAngle = ResetSteeringWheel(_steering, _zAngle);
            _steeringWheel.transform.localEulerAngles = new Vector3(0, 0, _zAngle);
            //_steeringWheel.transform.localEulerAngles = new Vector3(_zAngle, 90, -25);
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
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject.tag == _steeringWheelTag)
            {
                _currentlySteering = true;
                //StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }
    }

    int offset = 0;
    float speed = 10;
    private float _zAngle = 0;

    private IEnumerator DragUpdate(GameObject steerWheel)
    {
        float initialDistance = Vector3.Distance(steerWheel.transform.position, _mainCamera.transform.position);
        //Debug.Log(initialDistance);
        //float value = _mouseClick.ReadValue<float>();
        //Cursor.lockState = CursorLockMode.Locked;
        while (_mouseClick.ReadValue<float>() != 0)
        {
            //if (steerWheel.tag == _rotateTag) // temp things in case other objects can be dragged somehow
            //{
            //    Debug.Log(steerWheel.transform.position.x - Mouse.current.position.ReadValue().x);
            //    steerWheel.transform.localEulerAngles = new Vector3(_zAngle += 
            //        steerWheel.transform.localPosition.x - Mouse.current.position.ReadValue().x * Time.deltaTime * Time.deltaTime, 90, -25);
            //    //_mouseClick.ReadValue<float>()
            //    yield return _waitForFixedUpdate;
            //}
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            //Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Mouse.current.position.ReadValue().x,
            //    Mouse.current.position.ReadValue().y, steerWheel.transform.position.z));
            Vector3 direction = ray.GetPoint(initialDistance) - steerWheel.transform.position;
            //_zAngle = Vector3.Angle(direction, steerWheel.transform.position);
            //if (direction.z < 0)
            //{
            //    ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            //    direction = ray.GetPoint(initialDistance) - steerWheel.transform.position;
            //}
            //Debug.Log(direction + " - " + ray.GetPoint(initialDistance) + " - " + steerWheel.transform.position + " - " + Mouse.current.position.ReadValue());
            //steerWheel.transform.localEulerAngles = new Vector3(_zAngle += GetAngle(direction) * xspeddd, 90, -25);

            //if (steerWheel.transform.forward.z < 0)
            {
                _zAngle = -TurningAngle(direction, steerWheel.transform.forward.z);
            }

            //if (steerWheel.transform.forward.z < 0 && _steering == SteerDirection.Right)
            //{
            //    offset = -90;
            //}
            //else if (steerWheel.transform.forward.z < 0 && _steering == SteerDirection.Left)
            //{
            //    offset = 90;
            //}
            //else
            //{
            //    offset = 0;
            //}
            //else
            //{
            //    _zAngle = -TurningAngle(direction, steerWheel.transform.forward.z);
            //}
            //_zAngle = -TurningAngle(direction, steerWheel.transform.forward.z) * steerWheel.transform.forward.z;

            _carControl.turn = -_zAngle * Mathf.PI / 180;// * steerWheel.transform.forward.z;
            //Debug.Log(_zAngle);
            steerWheel.transform.localEulerAngles = new Vector3(0, 0, _zAngle);// * steerWheel.transform.forward.z);
            //  + offset);
            //steerWheel.transform.Rotate(steerWheel.transform.forward, _carControl.turn);
            //steerWheel.transform.rotation = new Quaternion(steerWheel.transform.localRotation.x, steerWheel.transform.localRotation.y, _zAngle, 1);
            //new Vector3(0, 0, steerWheel.transform.forward.z * -_zAngle);
            // new Vector3(0, 0, -_zAngle);
            if (direction.x >= 0)
            {
                
            }
            else
            {
                //steerWheel.transform.localEulerAngles = new Vector3(_zAngle, -90, -25);
            }
            
            //Debug.Log(steerWheel.transform.localEulerAngles.x);
            yield return _waitForFixedUpdate;
        }
        _currentlySteering = false;
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
            //radian = Mathf.Atan2(direction.x, -direction.y); // yx, xz yz zx zy, nästan ok: x,-y
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
        //Debug.Log(angle);
        if (steerDirection == SteerDirection.Left)
        {
            angle += _resetSpeed * Time.deltaTime;
        }
        else if (steerDirection == SteerDirection.Right)
        {
            angle -= _resetSpeed * Time.deltaTime;
        }
        if (angle > -_steerOffset && angle < _steerOffset)
        {
            _steering = SteerDirection.Straight;
            _carControl.turn = 0;
            return 0;
        }
        _carControl.turn = (Mathf.PI / 180) * angle;
        return angle;
    }
}
