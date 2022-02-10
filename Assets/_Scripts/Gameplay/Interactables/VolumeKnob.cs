using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeKnob : Lever
{
    [SerializeField] private float _minVol;
    [SerializeField] private float _maxVol;

    [SerializeField] private AudioMixer _radioMixer;

    public override void Start()
    {
        var musicVol = Mathf.Clamp(GetMusicVol(), _minVol, _maxVol);
        _leverValue = Mathf.Abs(musicVol - _minVol) / Mathf.Abs(_maxVol - _minVol); 
        _orgin.rotation = LongLerp(_start.rotation, _end.rotation, _curve.Evaluate(_leverValue));
        
    }

    public override void UpdateLeverTransform()
    {
        _orgin.rotation = LongLerp(_start.rotation, _end.rotation, _curve.Evaluate(_leverValue));
        
        // Messy to put here, but method is called as the levervalue is set in the baseclass
        ChangeVolume();
    }
    
    private void ChangeVolume()
    {
        var radioVolume = Mathf.Lerp(_minVol, _maxVol, _leverValue);
        _radioMixer.SetFloat("MusicVol", radioVolume); 
        
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

    public float GetMusicVol()
    {
        float startVolume; 
        bool result = _radioMixer.GetFloat("MusicVol", out startVolume );
        if (result)
        {
            return startVolume;
        } 
        else return 0f; 
    }
}
