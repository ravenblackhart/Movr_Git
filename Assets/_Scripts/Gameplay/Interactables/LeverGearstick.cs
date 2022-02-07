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
        //_dragSpeed = 2f;
        //Debug.LogWarning("funkar>!!" + _playerDrag);
    }

    public override void UpdateLeverValue()
    {
        // How much does the lever value change, script GearStick reads it (LeverValue)
        _leverValue -= _moveDirection * Time.deltaTime;
        
        _leverValue = Mathf.Clamp(_leverValue, -0.5f, 1);
    }
    // clamp -60, -40,  -20,  0,   15    30    
    // value   1, 0.66, 0.33, 0, -0.25, -0.5
    public override void UpdateLeverTransform()
    {
        // Change the lever value to degrees
        _zAngle = _leverValue * _value2AngleModifier;
        _zAngle = Mathf.Clamp(_zAngle,-30f, 60f);
        // rotate the stick correctly
        _orgin.transform.localEulerAngles = new Vector3(_orgin.transform.localEulerAngles.x,
            _orgin.transform.localEulerAngles.y,  SetStickAngle( -_zAngle));//-_zAngle);//
        
    }

    private float SetStickAngle(float curValue)
    {
        if (curValue < -55)
        {
            _leverValue = 1;
            return -60;
        }
        else if (curValue < -25 && curValue > -35)
        {
            _leverValue = 0.5f;
            return -30;
        }
        else if (curValue < 5 && curValue > -5)
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
        //int arrayIndex = 0;
        //float prevGear = float.MinValue;
        //foreach (float a in _gearAngles)
        //{
        //    if (curValue <= a && curValue > prevGear)
        //    {
        //        //StartCoroutine(ResetStick());
        //        //_leverValue = SetLeverValue(arrayIndex);
        //        //return a;
        //        Debug.LogWarning(a + " " + prevGear + " " + curValue + " " + _moveDirection);
        //        float diffA = a - curValue;
        //        float diffB = curValue - prevGear;
        //        if (_moveDirection < 0)
        //        {
        //            //Debug.LogWarning(a + " a " + curValue);
        //            return a;
        //        }
        //        else if (_moveDirection > 0)
        //        {
        //            //Debug.LogWarning(prevGear + " b " + curValue);
        //            return prevGear;
        //        }
        //    }
        //    arrayIndex++;
        //    prevGear = a;
        //}
        //    Debug.LogWarning(curValue);
        //if (curValue < 0)
        //{
        //    return -60;
        //}
        //return 30;
    }

}
