using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class RadialTimer : MonoBehaviour
{
    [SerializeField] private float indicatorTimer = 1.0f;

    [SerializeField] private float maxIndicatorTimer = 1.0f;

    [SerializeField] private Image radialIndicatorUI;
    [SerializeField] private Color32 color1 = Color.green;
    [SerializeField] private Color32 color2 = Color.yellow;
    [SerializeField] private Color32 color3 = Color.red; 

    [SerializeField] private UnityEvent myEvent = null;

    public bool shouldUpdate = false;
    private float passengerDelay = 0.3f; 
    
    
    public void Update()
    {
        if (Keyboard.current.gKey.isPressed)
        {
            shouldUpdate = false; 
            indicatorTimer -= (Time.deltaTime * passengerDelay);
            radialIndicatorUI.enabled = true;
            radialIndicatorUI.color = color1;
            radialIndicatorUI.fillAmount = indicatorTimer;

            if (radialIndicatorUI.fillAmount < 0.65)
            {
                radialIndicatorUI.color = color2; 
            }
            
            if (radialIndicatorUI.fillAmount < 0.35)
            {
                radialIndicatorUI.color = color3; 
            }

            if (indicatorTimer <= 0)
            {
                indicatorTimer = maxIndicatorTimer;
                radialIndicatorUI.fillAmount = maxIndicatorTimer;
                radialIndicatorUI.enabled = false; 
                myEvent.Invoke();
            }
            
            
        }

        else
        {
            if (shouldUpdate)
            {
                indicatorTimer += Time.deltaTime;
                radialIndicatorUI.fillAmount = indicatorTimer;

                if (indicatorTimer >= maxIndicatorTimer)
                {
                    indicatorTimer = maxIndicatorTimer;
                    radialIndicatorUI.fillAmount = maxIndicatorTimer;
                    radialIndicatorUI.enabled = false;
                    shouldUpdate = false; 
                }
            }
        }

        if (Keyboard.current.aKey.wasReleasedThisFrame) shouldUpdate = true; 
    }
    
}
