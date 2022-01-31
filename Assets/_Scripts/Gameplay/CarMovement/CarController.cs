using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public enum Direction { Left, Right, Forward }
    [Header("How many axles the car will have")]
    [SerializeField]
    private List<AxleInfo> _axleInfos; // the information about each individual axle
    [SerializeField]
    private float _maxMotorTorque = 50; // maximum torque the motor can apply to wheel
    [SerializeField]
    private float _maxSteeringAngle = 25; // maximum steer angle the wheel can have

    [SerializeField]
    private float _maxBrakeTorque = 8000; // maximum brake torque the motor can apply to wheel
    private float _curBrakeTorque = 0;

    private GameObject _steeringWheel;


    [Range(-1, 1)]
    public float speed = 1;
    [Range(-1, 1)]
    public float turn = 0;

    private float _zAngle = 0;
    

    // Start is called before the first frame update
    void Awake()
    {
        _steeringWheel = GameObject.FindGameObjectWithTag("SteeringWheel");
    }
    
    void FixedUpdate()
    {

        // // if (Keyboard.current.wKey.isPressed && _speed < 1)
        // // {
        // //     if (_speed < 0)
        // //     {
        // //         _speed = 0;
        // //     }
        // //     _speed += Time.deltaTime;
        // }
        // // else if (Keyboard.current.sKey.isPressed && _speed > -1)
        // // {
        // //     if (_speed > 0)
        // //     {
        // //         _speed = 0;
        // //     }
        // //     _speed -= Time.deltaTime;
        // // }
        // // else

        // if (TurnLeft && turn > -1)
        // {
        //     turn -= Time.deltaTime;
        //     // _steeringWheel.transform.localEulerAngles = new Vector3(-10, 0, _zAngle -= turn * steerTempSpeed);
        // }
        // else if (!TurnLeft && turn < 1)
        // {
        //     turn += Time.deltaTime;
        //     // _steeringWheel.transform.localEulerAngles = new Vector3(-10, 0, _zAngle -= turn * steerTempSpeed);
        // }

        // else if (!TurnLeft && !TurnRight)
        // {
        //     if (turn < 0)
        //     {
        //         turn += Time.deltaTime;
        //     }
        //     else
        //     {
        //         turn -= Time.deltaTime;
        //     }
        //
        //     _steeringWheel.transform.localEulerAngles = new Vector3(-10, 0, _zAngle += turn * steerTempSpeed);
        //     if (turn < 0.01f && turn > -0.01f)
        //     {
        //         turn = 0;
        //         _zAngle = 0;
        //         _steeringWheel.transform.localEulerAngles = new Vector3(-10, 0, 0);
        //     }
        // }
        
        // // Hacky temp solution to 
        //  float recoverytime = 2;
        //  if (!(_rigidbody.velocity.sqrMagnitude < _maxVelocity))
        //      speed = Mathf.MoveTowards(speed, 0, recoverytime * Time.deltaTime);
        //  else
        //  {
        //      speed = Mathf.MoveTowards(speed, 1, recoverytime * Time.deltaTime);
        //  }
        //
        
        
        float motor = _maxMotorTorque * speed * Time.deltaTime;
        float steering = _maxSteeringAngle * turn;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Braking(float time)
    {
        StartCoroutine(StartBraking(time));
    }

    private IEnumerator StartBraking(float time)
    {
        _curBrakeTorque = _maxBrakeTorque;
        // The shorter time the better, pretty much determines braking time
        yield return new WaitForSeconds(time);
        _curBrakeTorque = 0;
    }

    // Finds the corresponding visual wheel and, beh�vs detta?
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
        [Header("Is this axle attached to motor?")]
        public bool motor; 
        [Header("Will this axle be steerable?")]
        public bool steering; 
    }

}

