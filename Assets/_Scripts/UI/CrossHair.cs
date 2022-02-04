using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    #region Declarations

    public static CrossHair Instance; 
    
    [Header("Crosshair Sprites")] 
    public Sprite DefaultCrosshair;
    public Sprite PickupsCrosshair;
    public Sprite ButtonsCrosshair;
    public Sprite SteeringCrosshair;

    [Header("Image Renderer")]
    [SerializeField] private Image imageRenderer; 
    
    #endregion


    private void Awake()
    { 
        if (Instance == null)
        Instance = this; 
    }
    private void Start()
    {
        ResetCrosshair();
    }

    public void UpdateCrosshair(GameObject other)
    {
        if (other.TryGetComponent(out PhysicsObject phys) || other.TryGetComponent(out Lever lev))
        {
            imageRenderer.sprite = PickupsCrosshair;
            imageRenderer.preserveAspect = true;
        }
        else if (other.TryGetComponent(out Button but))
        {
            imageRenderer.sprite = ButtonsCrosshair;
            imageRenderer.preserveAspect = true;
        }       
        
        else if (other.TryGetComponent(out SteeringWheelInteactable steer))
        {
            imageRenderer.sprite = SteeringCrosshair;
            imageRenderer.preserveAspect = true;
        }
        else ResetCrosshair();
        
    }

    public void ResetCrosshair()
    {
        imageRenderer.sprite = DefaultCrosshair; 
        imageRenderer.preserveAspect = true;
    }
}
