using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Lever))]
public class Gearstick : MonoBehaviour
{

    private CarController _carController;

    private LeverGearstick _gearLever;

    private bool _forward = true;
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
    void Update()
    {
        _carController.speed = _gearLever.LeverValue;
        if (_gearLever.CurrentGear > 0 && !_forward)
        {
            ChangeDirectionSpeed();
        }
        else if (_forward && _gearLever.CurrentGear < 0)
        {
            ChangeDirectionSpeed();
        }
        else if (_gearLever.CurrentGear == 0 && !_stopped)
        {
            GoNeutral();
        }
    }

    private void GoNeutral()
    {
        _stopped = true;
        _carController.speed = 0;
        _carController.Braking(0.3f);
    }

    private void ChangeDirectionSpeed()
    {
        _stopped = false;
        _carController.Braking(0.2f);
        _forward = !_forward;
        _carController.isReversing = !_carController.isReversing;
    }
}
