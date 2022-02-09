using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCrossHairOnRayCast : MonoBehaviour, IInteractable
{
    private float _maxRange = 10f;
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
