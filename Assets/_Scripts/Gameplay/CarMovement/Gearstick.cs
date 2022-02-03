using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// todo:
//[RequireComponent(typeof(Lever))]
public class Gearstick : MonoBehaviour//, IInteractable
{
    /// <summary>
    /// Interface stuff:
    /// </summary>

    [SerializeField] private float _maxRange = 20;
    public float MaxRange => _maxRange;

    //public void OnStartHover()
    //{
    //    //print("Started Hover");
    //}

    //public void OnInteract()
    //{
    //    MoveStick();
    //    _carController.Braking(0.2f);
    //    _forward = !_forward;
    //    //Debug.Log(_carController.speed + " fwrd: " + _forward);
    //}

    //public void OnEndHover()
    //{
    //    //print("Ended Hover");
    //}

    //////////////////////// interface stuff end


    private CarController _carController;

    private Lever _gearLever;
    //[SerializeField]
    private float _reverseLimit = 0.95f;
    //[SerializeField]
    private float _forwardLimit = 0.05f;

    private bool _forward = true;

    // Direction of vehicle, positive (1) is forward, negative is reverse 
    private float _vehicleDirection = 1;

    private float _reverseGearStrength = -1;

    [SerializeField]
    private string _carControllerTag = "CarDriving";

    

    // Start is called before the first frame update
    void Awake()
    {
        _carController = GameObject.FindGameObjectWithTag(_carControllerTag).GetComponent<CarController>();
        
        _gearLever = GetComponent<Lever>();
        float temp = _gearLever.LeverValue;
    }
    float temp;
    // Update is called once per frame
    void Update()
    {
        //if (Keyboard.current.wKey.isPressed)
        //{
        //    Debug.Log("lower" + temp);
        //    temp -= Time.deltaTime;
        //}
        //if (Keyboard.current.aKey.isPressed)
        //{
        //    Debug.Log("higher" + temp);

        //    temp += Time.deltaTime;
        //}
        //if (temp > _reverseLimit && _forward)
        //{
        //    ChangeDirectionSpeed(_reverseGearStrength);
        //}
        //else if (!_forward && temp < _forwardLimit)
        //{
        //    ChangeDirectionSpeed(1);
        //}
        if (_gearLever.LeverValue > _reverseLimit && _forward)
        {
            ChangeDirectionSpeed(_reverseGearStrength);
            _forward = false;
        }
        else if (!_forward && _gearLever.LeverValue < _forwardLimit)
        {
            ChangeDirectionSpeed(1);
            _forward = true;
        }
        if (_vehicleDirection > _reverseGearStrength && !_forward)
        {
            ChangeDirectionSpeed(_reverseGearStrength);
        }
        else if (_forward && _vehicleDirection < 0)
        {
            ChangeDirectionSpeed(1);
        }
    }

    private void ChangeDirectionSpeed(float newValue)
    {
        _carController.Braking(0.2f);
        _forward = !_forward;
        _vehicleDirection = newValue;
        _carController.isReversing = !_carController.isReversing;
    }
}
