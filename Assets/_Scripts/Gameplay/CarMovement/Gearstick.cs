using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gearstick : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Interface stuff:
    /// </summary>

    [SerializeField] private float _maxRange = 20;
    public float MaxRange => _maxRange;

    public void OnStartHover()
    {
        //print("Started Hover");
    }

    public void OnInteract()
    {
        MoveStick();
        _carController.Braking();
        _forward = !_forward;
    }

    public void OnEndHover()
    {
        //print("Ended Hover");
    }

    //////////////////////// interface stuff end


    [SerializeField]
    private CarController _carController;

    private bool _forward = true;

    // Direction of vehicle, positive (1) is forward, negative is reverse 
    private float _direction = 1;

    [Range(-1, -0.1f)]
    [SerializeField]
    private float _reverseGearStrength = -0.5f;

    [SerializeField]
    private string _carNameWithCarController = "CarDriving";

    // Start is called before the first frame update
    void Start()
    {
        if (_carController == null)
        {
            _carController = GameObject.Find(_carNameWithCarController).GetComponent<CarController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_direction > _reverseGearStrength && !_forward)
        {
            ChangeDirectionSpeed(_reverseGearStrength);
        }
        else if (_forward && _direction < 0)
        {
            ChangeDirectionSpeed(1);
        }
    }

    private void MoveStick()
    {
        float xAngle;
        if (!_forward)
        {
            xAngle = -35;
        }
        else
        {
            xAngle = -70;
        }
        transform.localEulerAngles = new Vector3(xAngle, 0, 0);
    }

    private void ChangeDirectionSpeed(float newValue)
    {
        _direction = newValue;
        _carController.speed = _direction;
    }
}
