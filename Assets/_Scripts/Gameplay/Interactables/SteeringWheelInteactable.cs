using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheelInteactable : MonoBehaviour, IInteractable
{
    //TODO change and add to exisitng script
    [SerializeField] private float _maxRange = 10f;
    public float MaxRange => _maxRange;
    
    public void OnStartHover()
    {
        CrossHair.Instance.UpdateCrosshair(gameObject);
    }

    public void OnInteract()
    {
        
    }

    public void OnEndHover()
    {
        CrossHair.Instance.ResetCrosshair();
    }
}
