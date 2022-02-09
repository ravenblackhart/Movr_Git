using System;
using UnityEngine;

public class WaterBottle : PhysicsObject
{
    [SerializeField] private ParticleSystem _particleSystem;
    
    private void Update()
    {
        if (!beingHeld)
        {
            if (_particleSystem.isPlaying) 
                _particleSystem.Stop();
            
            return;
        }

        if (!_particleSystem.isPlaying) 
                _particleSystem.Play();
    }
}
