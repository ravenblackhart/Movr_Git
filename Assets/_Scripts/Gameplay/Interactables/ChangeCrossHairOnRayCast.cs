using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCrossHairOnRayCast : MonoBehaviour, IInteractable
{
    private float _maxRange = 10f;
    public float MaxRange => _maxRange;

    public bool draggingWheel;

    public bool lookingAtWheel;

    public void OnStartHover()
    {
        lookingAtWheel = true;
        CrossHair.Instance.UpdateCrosshair(gameObject);
    }

    public void OnInteract()
    {
    }

    public void OnEndHover()
    {
        lookingAtWheel = false;

        if (!draggingWheel) {
            CrossHair.Instance.ResetCrosshair();
        }
    }
}
