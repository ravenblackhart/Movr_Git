using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public enum Direction { Left, Right, Forward }
    [Header("How many axles will the car have, which will be steerable and motored")]
    [SerializeField]
    private List<AxleInfo> _axleInfos; // the information about each individual axle
    [SerializeField]
    private float _maxMotorTorque = 200; // maximum torque the motor can apply to wheel
    [SerializeField]
    private float _maxSteeringAngle = 25; // maximum steer angle the wheel can have


    [Range(-1, 1)]
    public float _speed = 1;
    [Range(-1, 1)]
    public float turn = 0;


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
        //else if (Keyboard.current.aKey.isPressed && _turn > -1)
        //{
        //    _turn -= Time.deltaTime;
        //}
        //else if (Keyboard.current.dKey.isPressed && _turn < 1)
        //{
        //    _turn += Time.deltaTime;
        //}
        //else if (!Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed)
        //{
        //    _turn = 0;
        //}

        float motor = _maxMotorTorque * _speed;// Input.GetAxis("Vertical");
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
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
