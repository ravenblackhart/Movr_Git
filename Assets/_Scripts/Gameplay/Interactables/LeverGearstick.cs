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

    void Awake()
    {
        _playerDrag = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDragObject>();
    }

    public override void UpdateLeverValue()
    {
        // How much does the lever value change depending on mouse movement,
        // script GearStick reads it (the LeverValue)
        _leverValue -= _moveDirection * Time.deltaTime;
        _leverValue = Mathf.Clamp(_leverValue, -0.5f, 1);
    }

    public override void UpdateLeverTransform()
    {
        // Change the lever value to degrees
        _zAngle = _leverValue * _value2AngleModifier;
        _zAngle = Mathf.Clamp(_zAngle,-30f, 60f);
        // rotate the stick correctly
        _orgin.transform.localEulerAngles = new Vector3(_orgin.transform.localEulerAngles.x,
            _orgin.transform.localEulerAngles.y,  SetStickAngle( -_zAngle));//-_zAngle);//
        
    }
    /// <summary>
    /// Gives the stick four gears: full speed, half speed, park(brake) and reverse
    /// </summary>
    /// <param name="curValue"></param>
    /// <returns></returns>
    private float SetStickAngle(float curValue)
    {
        if (curValue < -55)
        {
            _leverValue = 1;
            return -60;
        }
        else if (curValue < -25 && curValue > -40)
        {
            _leverValue = 0.5f;
            return -30;
        }
        else if (curValue < 10 && curValue > -5)
        {
            _leverValue = 0;
            return 0;
        }
        else if (curValue > 20)
        {
            _leverValue = -0.5f;
            return 30;
        }
        else
        {
            return curValue;
        }
    }

}
