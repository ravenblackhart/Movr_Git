using System;
using UnityEngine;

public class LeverGearstick : Lever
{
    private float _zAngle;
    [SerializeField]
    private float _value2AngleModifier = 60.0f;

    public override void UpdateLeverValue()
    {
        _leverValue -= _moveDirection * Time.deltaTime;
        
        //
        // _angle = Mathf.Clamp(_angle, -60, 60);

        
        // _leverValue += _moveDirection * _dragSpeed * Time.deltaTime;
        
        _leverValue = Mathf.Clamp(_leverValue, -0.5f, 1);
    }
    

    public override void UpdateLeverTransform()
    {

        // _orgin.position = Vector3.Lerp(_start.transform.position, _end.transform.position, _curve.Evaluate(_leverValue));
        // _orgin.rotation = Quaternion.Slerp(_start.rotation, _end.rotation, _curve.Evaluate(_leverValue));
        _zAngle = _leverValue * _value2AngleModifier;
        // -60 full frammåt, 60 full bakåt
        _zAngle = Mathf.Clamp(_zAngle,-60f, 60f);
        
        _orgin.transform.localEulerAngles = new Vector3(_orgin.transform.localEulerAngles.x, 
            _orgin.transform.localEulerAngles.y, -_zAngle);
        
    }
}
