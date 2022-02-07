using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeKnob : Lever
{
    public override void UpdateLeverTransform()
    {
        _orgin.rotation = LongLerp(_start.rotation, _end.rotation, _curve.Evaluate(_leverValue));
    }
    
    private Quaternion LongLerp(Quaternion p, Quaternion q, float t)
    {
        Quaternion r = Quaternion.identity;
        r.x = p.x * (1f - t) + q.x * (t);
        r.y = p.y * (1f - t) + q.y * (t);
        r.z = p.z * (1f - t) + q.z * (t);
        r.w = p.w * (1f - t) + q.w * (t);
        return r;
    }
}
