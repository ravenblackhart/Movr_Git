using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Lever))]
public class Gearstick : MonoBehaviour
{

    private CarController _carController;

    private LeverGearstick _gearLever;

    private enum Direction { Forward, Stopped, Reverse};

    private Direction _direction = Direction.Stopped;
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
            Debug.LogError("error: gearstick cant find its lever!!! chaos!!!!");
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //_carController.maxVelocity *= Mathf.Abs(_gearLever.LeverValue);
        if (_gearLever.CurrentGear > 0 && _direction != Direction.Forward)
        {
            ChangeDirectionSpeed(Direction.Forward);
        }
        else if (_gearLever.CurrentGear < 0 && _direction != Direction.Reverse)
        {
            ChangeDirectionSpeed(Direction.Reverse);
        }
        else if (_gearLever.CurrentGear == 0 && !_stopped)
        {
            StopCar();
        }
    }

    private void StopCar()
    {
        _direction = Direction.Stopped;
        _carController.Braking(0.2f);
        _stopped = true;
        _carController.speed = 0;
    }

    private void ChangeDirectionSpeed(Direction newDirection)
    {
        _carController.speed = _gearLever.LeverValue;
        _stopped = false;
        _carController.Braking(0.2f);
        _direction = newDirection;
        _carController.isReversing = !_carController.isReversing;
    }
}
