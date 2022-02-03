using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    #region Declarations

    [Header("Crosshair Sprites")] 
    public Sprite DefaultCrosshair;
    public Sprite PickupsCrosshair;
    public Sprite SteeringCrosshair;

    [Header("Image Renderer")]
    [SerializeField] private Image imageRenderer; 
    
    #endregion

    public void UpdateCrosshair(GameObject other)
    {
        if (other.gameObject.CompareTag("Steering"))
        {
            imageRenderer.sprite = SteeringCrosshair;
            imageRenderer.preserveAspect = true;
        }

        if (other.gameObject.CompareTag("Pickup"))
        {
            imageRenderer.sprite = PickupsCrosshair;
            imageRenderer.preserveAspect = true;
        } 
        
    }

    public void ResetCrosshair()
    {
        imageRenderer.sprite = DefaultCrosshair; 
        imageRenderer.preserveAspect = true;
    }
}
