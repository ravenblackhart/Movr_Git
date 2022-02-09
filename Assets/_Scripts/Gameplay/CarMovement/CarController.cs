using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public bool isReversing = false;

    [Range(-1, -0.1f)]
    [SerializeField]
    private float _reverseSpeed = -0.5f; // speed in reverse, -1 = same speed as forward

    [Header("How many axles the car will have")]
    [SerializeField]
    private List<AxleInfo> _axleInfos; // the information about each individual axle

    [SerializeField]
    private float _maxMotorTorque = 5000; // maximum torque the motor can apply to wheel
    [SerializeField]
    private float _maxSteeringAngle = 25; // maximum steer angle the wheel can have

    [SerializeField]
    private float _maxBrakeTorque = 80000; // maximum brake torque the motor can apply to wheel
    private float _curBrakeTorque = 0;

    [Range(-1, 1)]
    public float speed = 0;
    [Range(-1, 1)]
    public float turn = 0;


    private Rigidbody _rigidbody;
    public float maxVelocity = 50; // Maximum speed allowed
    private readonly float _recoverytime = 30;

    //private bool motorSound = false;
    private Sound _motorSound;
    private readonly string _driving = "Driving";

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _motorSound = FindMotorSound(_driving);
        _motorSound.loop = true;
        AudioManager.Instance.Play(_driving);
        _motorSound.source.Pause();
    }
    
    private Sound FindMotorSound(string name)
    {
        return Array.Find(AudioManager.Instance.sounds, sound => sound.name == name);
    }

    void FixedUpdate()
    {
        // prevent the car from moving to quickly
        float maxSpeed = maxVelocity;
        if (isReversing)
        {
            // Change the top speed depending on how quickly the car can go in reverse
            maxSpeed *= Mathf.Abs(_reverseSpeed);
        }
        if (_rigidbody.velocity.sqrMagnitude > maxSpeed)
        {
            // Slow down the car
            speed = GetSpeed(true, isReversing);
        }
        
        // Move and steer the car
        float motor = _maxMotorTorque * speed * Time.deltaTime;
        float steering = _maxSteeringAngle * turn;

        // sound if car is moving
        //if ((motor > 0.5f || motor < -0.5f) && !_motorSound.source.isPlaying)
        //{
        //    AudioManager.Instance.Play("Driving");
        //}
        //else 
        if ((motor > 0.5f || motor < -0.5f) && !_motorSound.source.isPlaying)
        {
            _motorSound.source.UnPause();
        }
        else if (motor < 0.5f && motor > -0.5f)
        {
            //_motorSound.source.Pause();
        }

        //if (!motorSound && (int)speed != 0)
        //{
        //    AudioManager.Instance.Play("Driving");
        //    motorSound = true;
        //}
        //else if ((int)speed == 0)
        //{
        //    motorSound = false;
        //}
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

    private float GetSpeed(bool decrease, bool reversing)
    {
        float speed = 0;
        float minTarget = 0;
        float maxTarget = 1;
        // Changing these values if its reversing 
        if (reversing)
        {
            speed = _reverseSpeed;
            minTarget = 0;
            maxTarget = _reverseSpeed;
        }
        if (decrease)
        {
            speed = Mathf.MoveTowards(speed, minTarget, _recoverytime * Time.deltaTime);
        }
        else
        {
            speed = Mathf.MoveTowards(speed, maxTarget, _recoverytime * Time.deltaTime);
        }
        return speed;
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

    // Finds the corresponding visual wheel and, behövs detta?
    // correctly turns them
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

    [Serializable]
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

