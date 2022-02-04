using System;
using UnityEngine;

public class LeverGearstick : Lever
{
    // public override void UpdateLeverValue()
    // {
    //     _leverValue += _moveDirection * Time.deltaTime;
    //     // _leverValue += _moveDirection * _dragSpeed * Time.deltaTime;
    //     _leverValue = Mathf.Clamp(_leverValue, -1, 1);
    // }
    //
    // public override void UpdateLeverTransform()
    // {
    //     // _orgin.position = Vector3.Lerp(_start.transform.position, _end.transform.position, _curve.Evaluate(_leverValue));
    //     _orgin.rotation = Quaternion.Slerp(_start.rotation, _end.rotation, _curve.Evaluate(_leverValue));
    // }
}
