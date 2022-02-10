using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Lever))]
public class Gearstick : MonoBehaviour
{

    private CarController _carController;

    private LeverGearstick _gearLever;

    private bool _stopped = true;

    private readonly string _carControllerTag = "CarDriving";

    // Start is called before the first frame update
    void Awake()
    {
        _carController = GameObject.FindGameObjectWithTag(_carControllerTag).GetComponent<CarController>();
        if (TryGetComponent<LeverGearstick>(out _gearLever))
        {
            _gearLever = GetComponent<LeverGearstick>();
        }
        else
        {
            Debug.LogError("error: gearstick cant find its lever!!! chaos!!! PANIC NOWW!!!!1!!!!1!");
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (_gearLever.CurrentGear > 0 && _carController._direction != CarController.Direction.Forward)
        {
            ChangeDirectionSpeed(CarController.Direction.Forward);
        }
        else if (_gearLever.CurrentGear < 0 && _carController._direction != CarController.Direction.Reverse)
        {
            ChangeDirectionSpeed(CarController.Direction.Reverse);
        }
        else if (_gearLever.CurrentGear == 0 && !_stopped)
        {
            StopCar();
        }
    }

    private void StopCar()
    {
        _carController._direction = CarController.Direction.Stopped;
        _carController.Braking(0.2f);
        _stopped = true;
    }

    private void ChangeDirectionSpeed(CarController.Direction newDirection)
    {
        _carController.speed = _gearLever.LeverValue;
        //Debug.LogWarning(newDirection + " " + _carController.speed);
        _stopped = false;
        _carController.Braking(0.2f);
        _carController._direction = newDirection;
    }
}
