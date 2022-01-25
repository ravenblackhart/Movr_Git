using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    #region Declarations

    [Header("Crosshair Sprites")] 
    [SerializeField] private Sprite defaultCrosshair;
    [SerializeField] private Sprite pickupsCrosshair;
    [SerializeField] private Sprite interactCrosshair;

    [Header("Image Renderer")]
    [SerializeField] private Image imageRenderer; 
    
    #endregion

    public void UpdateCrosshair(GameObject other)
    {
        if (other.gameObject.CompareTag("Interact")) imageRenderer.sprite = interactCrosshair;

        if (other.gameObject.CompareTag("Pickup")) imageRenderer.sprite = pickupsCrosshair; 
    }

    public void ResetCrosshair()
    {
        imageRenderer.sprite = defaultCrosshair; 
    }
}
