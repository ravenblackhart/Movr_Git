using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverGearstick : Lever
{
    private float _zAngle;
    [Header("Keep this in degrees and so it matches the stick object")]
    [SerializeField]
    private float _value2AngleModifier = 60.0f;

    private readonly string _audioName = "Gearstick";

    private int _currentGear = 0;
    public int CurrentGear => _currentGear;

    private CarController _carController;
    private readonly string _carControllerTag = "CarDriving";

    private float _maxSpeed;

    void Awake()
    {
        _playerDrag = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDragObject>();
        _carController = GameObject.FindGameObjectWithTag(_carControllerTag).GetComponent<CarController>();
        _maxSpeed = _carController.maxVelocity;
        _dragSpeed = 2f;
    }

    public override void UpdateLeverValue()
    {
        // How much does the lever value change depending on mouse movement,
        // script GearStick reads it (the LeverValue)
        _leverValue -= _moveDirection * Time.deltaTime;
        _leverValue = Mathf.Clamp(_leverValue, -0.5f, 1);
        if (_leverValue > 0)
        {
            _carController.maxVelocity = _maxSpeed * _leverValue;
        }
        else
        {
            _carController.maxVelocity = _maxSpeed;
        }
    }

    public override void UpdateLeverTransform()
    {
        // Change the lever value to degrees
        _zAngle = _leverValue * _value2AngleModifier;
        _zAngle = Mathf.Clamp(_zAngle,-30f, 60f);
        // rotate the stick correctly
        _orgin.transform.localEulerAngles = new Vector3(_orgin.transform.localEulerAngles.x,
            _orgin.transform.localEulerAngles.y,  SetStickAngle( -_zAngle));
        if (-_zAngle < -5 && -_zAngle > -55 && _currentGear != 1)
        {
            _currentGear = 1;
        }
        else if (-_zAngle > 5 && -_zAngle < 25)
        {
            _currentGear = -1;
        }
    }

    /// <summary>
    /// Gives the stick three gears: full speed, half speed, 
    /// park(brake) and reverse
    /// </summary>
    /// <param name="curAngle"></param>
    /// <returns></returns>
    private float SetStickAngle(float curAngle)
    {
        if (curAngle < -55)
        {
            return ChangeGearValues(2);
        }
        else if (curAngle < 5 && curAngle > -5)
        {
            return ChangeGearValues(0);
        }
        else if (curAngle > 25)
        {
            return ChangeGearValues(-1);
        }
        else
        {
            return curAngle;
        }
    }

    private int ChangeGearValues(int gear)
    {
        switch (gear)
        {
            case 2:
                ChangeGearSound(2);
                _leverValue = 1;
                return -60;
            case 1:
                ChangeGearSound(1);
                _leverValue = 0.5f;
                return -30;
            case 0:
                ChangeGearSound(0);
                _leverValue = 0;
                return 0;
            case -1:
                ChangeGearSound(-1);
                _leverValue = -0.5f;
                return 30;
            default:

                _leverValue = 0;
                return 0;
        }
    }

    private void ChangeGearSound(int gear)
    {
        // check if its not the same gear, only when changing to a new gear will it generate a sound
        if (gear != _currentGear)
        {
            AudioManager.Instance.Play(_audioName);
        }
        _currentGear = gear;
    }
}
