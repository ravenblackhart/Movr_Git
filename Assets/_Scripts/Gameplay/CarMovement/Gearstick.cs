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

    private LeverGearstick _gearLever;
    //[SerializeField]
    private float _reverseLimit = 0.95f;
    //[SerializeField]
    private float _forwardLimit = 0.05f;

    private bool _forward = true;
    private bool _stopped = true;

    [SerializeField]
    private float _neutralOffset = 0.1f;

    private float _reverseGearStrength = -1;

    private string _carControllerTag = "CarDriving";

    

    // Start is called before the first frame update
    void Awake()
    {
        _carController = GameObject.FindGameObjectWithTag(_carControllerTag).GetComponent<CarController>();
        if (TryGetComponent<LeverGearstick>(out _gearLever))
        {
            _gearLever = GetComponent<LeverGearstick>();
            tempLeverValue = _gearLever.LeverValue;
        }
        else
        {
            Debug.LogError("error: gearstick cant find its lever!!! chaos!!!!");
        }
        
    }
    private float tempLeverValue;
    // Update is called once per frame
    void Update()
    {
        //if (Keyboard.current.wKey.isPressed)
        //{
        //    temp += Time.deltaTime;
        //    Debug.LogWarning("lower" + temp);
        //}
        //if (Keyboard.current.aKey.isPressed)
        //{
        //    temp -= Time.deltaTime;
        //    Debug.LogWarning("higher" + temp);
        //}
        tempLeverValue = _gearLever.LeverValue;
        tempLeverValue = Mathf.Clamp(tempLeverValue, -0.5f, 1); // behövs?
        _carController.speed = tempLeverValue;
        if (tempLeverValue < -_neutralOffset && _forward)
        {
            ChangeDirectionSpeed();
        }
        else if (!_forward && tempLeverValue > _neutralOffset)
        {
            ChangeDirectionSpeed();
        }
        else if (tempLeverValue < _neutralOffset && tempLeverValue > -_neutralOffset && !_stopped)
        {
            GoNeutral();
            //Debug.LogWarning("stanna!!!");
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
