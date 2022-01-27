using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public enum Direction { Left, Right, Forward }
    [Header("How many axles will the car have and which will be steerable and motored")]
    [SerializeField]
    private List<AxleInfo> _axleInfos; // the information about each individual axle
    [SerializeField]
    private float _maxMotorTorque = 100; // maximum torque the motor can apply to wheel
    [SerializeField]
    private float _maxSteeringAngle = 25; // maximum steer angle the wheel can have

    [SerializeField]
    private float _maxBrakeTorque = 800; // maximum brake torque the motor can apply to wheel
    private float _curBrakeTorque = 0;
    private bool _braking = false;

    [SerializeField]
    private GameObject _steeringWheel;

    [SerializeField]
    private float steerTempSpeed = 50f;

    [Range(-1, 1)]
    public float speed = 1;
    [Range(-1, 1)]
    public float turn = 0;

    private float _zAngle = 0;


    [SerializeField]
    private float _turnSpeedModifier = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void FixedUpdate()
    {
        //if (Keyboard.current.wKey.isPressed && _speed < 1)
        //{
        //    if (_speed < 0)
        //    {
        //        _speed = 0;
        //    }
        //    _speed += Time.deltaTime;
        //}
        //else if (Keyboard.current.sKey.isPressed && _speed > -1)
        //{
        //    if (_speed > 0)
        //    {
        //        _speed = 0;
        //    }
        //    _speed -= Time.deltaTime;
        //}
        //else
        if (Keyboard.current.aKey.isPressed && turn > -1)
        {
            turn -= Time.deltaTime;
            _steeringWheel.transform.localEulerAngles = new Vector3(-10, 0, _zAngle -= turn * steerTempSpeed);
        }
        else if (Keyboard.current.dKey.isPressed && turn < 1)
        {
            turn += Time.deltaTime;
            _steeringWheel.transform.localEulerAngles = new Vector3(-10, 0, _zAngle -= turn * steerTempSpeed);
        }
        else if (!Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed)
        {
            if (turn < 0)
            {
                turn += Time.deltaTime;
            }
            else
            {
                turn -= Time.deltaTime;
            }
            _steeringWheel.transform.localEulerAngles = new Vector3(-10, 0, _zAngle += turn * steerTempSpeed);
            if (turn < 0.01f && turn > -0.01f)
            {
                turn = 0;
                _zAngle = 0;
                _steeringWheel.transform.localEulerAngles = new Vector3(-10, 0, 0);
            }
        }
        if (!_braking)
        {
            float motor = _maxMotorTorque * speed;// Input.GetAxis("Vertical");
            float steering = _maxSteeringAngle * turn * _turnSpeedModifier;// Input.GetAxis("Horizontal");

            foreach (AxleInfo axleInfo in _axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                    axleInfo.leftWheel.brakeTorque = _curBrakeTorque;
                    axleInfo.rightWheel.brakeTorque = _curBrakeTorque;
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Braking()
    {
        StartCoroutine(StartBraking());
    }

    private IEnumerator StartBraking()
    {
        _braking = true;
        _curBrakeTorque = _maxBrakeTorque;
        yield return new WaitForSeconds(0.1f); // The shorter time the better, pretty much det. braking time
        _curBrakeTorque = 0;
        _braking = false;
    }

    // Finds the corresponding visual wheel and, behövs detta?
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor; // is this wheel attached to motor?
        public bool steering; // does this wheel apply steer angle?
    }
}
