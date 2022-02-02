using System;
using UnityEngine;

public class WaterBottle : PhysicsObject
{
    [SerializeField] private ParticleSystem _particleSystem;
    
    private void Update()
    {
        if (Vector3.Dot(transform.up, Vector3.up) < -0.5f)
        {
            if (_particleSystem != null && !_particleSystem.isPlaying)
                _particleSystem.Play();
        }
        else
        {
            if (_particleSystem.isPlaying)
            {
                _particleSystem.Stop();
            }
        }
    }
}
