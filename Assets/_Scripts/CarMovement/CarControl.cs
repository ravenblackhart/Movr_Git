using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarControl : MonoBehaviour
{
    public enum Direction { Left, Right, Forward }

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque = 400; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle = 30; // maximum steer angle the wheel can have

    public GameObject[] wheels = new GameObject[4];

    public int Speed { get; set; }
    public int TopSpeed { get; set; }
    public bool IsMoving { get; set; }



    // Start is called before the first frame update
    void Start()
    {
        Speed = 50;
        IsMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving)
        {
            //SpinWheels();
        }
    }

    private float rotation = 0;

    void SpinWheels()
    {
        foreach (GameObject wheel in wheels)
        {
            wheel.transform.eulerAngles = new Vector3(rotation, 0, 90);
            rotation += Time.deltaTime * Speed;
        }
    }


    public void Drive()
    {

    }

    public void Brake()
    {

    }

    public void Turn(Direction direction)
    {

    }

    [Range(-1,1)]
    public float speed = 1;
    [Range(-1, 1)]
    public float turn = 0;

    void FixedUpdate()
    {
        if (Keyboard.current.wKey.isPressed && speed < 1)
        {
            speed += Time.deltaTime;
        }
        else if (Keyboard.current.sKey.isPressed && speed > -1)
        {
            if (speed > 0)
            {
                speed = 0;
            }
            speed -= Time.deltaTime;
        }
        else if (Keyboard.current.aKey.isPressed && turn > -1)
        {
            turn -= Time.deltaTime;
        }
        else if (Keyboard.current.dKey.isPressed && turn < 1)
        {
            turn += Time.deltaTime;
        }
        else if (!Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed)
        {
            turn = 0;
        }
        //var inputMapper = new InputActionMap();
        //var myInputAction = inputMapper.AddAction("Horizontal");

        //myInputAction.AddCompositeBinding("Axis")
        //.With("Negative", < Keyboard >/ a)
        //.With("Positive", < Keyboard >/ d);

        //myInputAction.passThrough = true;

        float motor =  maxMotorTorque * speed;// Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * turn;// Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
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

    // finds the corresponding visual wheel
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
