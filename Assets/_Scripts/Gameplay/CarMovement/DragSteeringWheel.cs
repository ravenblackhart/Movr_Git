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
    private Camera _mainCamera;

    private CarController _carControl;

    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    private void Awake()
    {
        _mainCamera = Camera.main;
        _carControl = GetComponent<CarController>();
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
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }
        //Cursor.lockState = CursorLockMode.None;
    }

    float speed = 10;
    private float _xAngle = 0;

    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        float initialDistance = Vector3.Distance(clickedObject.transform.position, _mainCamera.transform.position);
        //float value = _mouseClick.ReadValue<float>();
        //Cursor.lockState = CursorLockMode.Locked;
        while (_mouseClick.ReadValue<float>() != 0)
        {
            //if (clickedObject.tag == _rotateTag) // temp things in case other objects can be dragged somehow
            //{
            //    Debug.Log(clickedObject.transform.position.x - Mouse.current.position.ReadValue().x);
            //    clickedObject.transform.localEulerAngles = new Vector3(_xAngle += 
            //        clickedObject.transform.localPosition.x - Mouse.current.position.ReadValue().x * Time.deltaTime * Time.deltaTime, 90, -25);
            //    //_mouseClick.ReadValue<float>()
            //    yield return _waitForFixedUpdate;
            //}
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Vector3 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
            //Debug.Log(direction);
            //clickedObject.transform.localEulerAngles = new Vector3(_xAngle += GetAngle(direction) * xspeddd, 90, -25);
            _xAngle = GetAngle(direction);
            clickedObject.transform.localEulerAngles = new Vector3(_xAngle, 90, -25);
            
            //Debug.Log(clickedObject.transform.localEulerAngles.x);
            yield return _waitForFixedUpdate;
        }
    }

    private float GetAngle(Vector3 direction)
    {
        _carControl.turn = Mathf.Atan2(direction.x, direction.y) / Mathf.PI;
        //Debug.Log(_carControl.turn);
        //return Mathf.Atan2(direction.x, direction.y) * (180 / Mathf.PI);
        return Mathf.Atan2(direction.x, direction.y) * (180 / Mathf.PI);


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
}
