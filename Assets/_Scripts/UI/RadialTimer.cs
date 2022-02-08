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
    public bool taskComplete = false; 
    private float passengerDelay = 0.3f;
    [HideInInspector] public float taskDuration = 10f;

    float fillProgress;
    float transitionProgress;
    bool transitionGoal;

    private void Start()
    {
        radialIndicatorUI.fillAmount = maxIndicatorTimer;
    }

    public void UpdateRadialTimer(float fill)
    {
        radialIndicatorUI.fillAmount = fill;

        fillProgress = fill;

        if (radialIndicatorUI.fillAmount > 0.5f)
        {
            radialIndicatorUI.color = CustomClasses.RemapLerp(color1, color2, 1f, 0.5f, fill);
        }
        else
        {
            radialIndicatorUI.color = CustomClasses.RemapLerp(color2, color3, 0.5f, 0f, fill);
        }

        Vector3 posDeltaShaking = (Vector3.right * Mathf.Sin(Time.time * 90f) * 2f + Vector3.up * Mathf.Cos(Time.time * 20f)) * Mathf.InverseLerp(0.3f, 0f, fill);

        radialIndicatorUI.transform.localPosition = posDeltaShaking + GetProgressPosition(transitionProgress);
    }

    public void UpdateRadialTimer()
    {
        if (transitionGoal)
        {
            transitionProgress = Mathf.Min(transitionProgress + Time.deltaTime / 0.5f, 1f);
        }
        else if (transitionProgress != 0f)
        {
            transitionProgress += Time.deltaTime / 0.5f;

            if (transitionProgress >= 2f)
            {
                transitionProgress = 0f;
            }
        }

        radialIndicatorUI.transform.localPosition = GetProgressPosition(transitionProgress);
    }

    public void UpdateRadialTimerGoal(bool goal)
    {
        transitionGoal = goal;
    }

    Vector3 GetProgressPosition(float progress)
    {
        return Vector3.up * Mathf.Pow(progress - 1f, 2f) * 200f;
    }

    void Update()
    {
        //if (Keyboard.current.gKey.isPressed)
        //{
        //    shouldUpdate = false;
        //    radialIndicatorUI.enabled = true;
        //    StartCoroutine(StartCountdown(taskDuration)); 
        //    radialIndicatorUI.color = color1;

        //    if (radialIndicatorUI.fillAmount < 0.65)
        //    {
        //        radialIndicatorUI.color = color2; 
        //    }
            
        //    if (radialIndicatorUI.fillAmount < 0.35)
        //    {
        //        radialIndicatorUI.color = color3; 
        //    }

        //    if (indicatorTimer <= 0)
        //    {
        //        indicatorTimer = maxIndicatorTimer;
        //        radialIndicatorUI.fillAmount = maxIndicatorTimer;
        //        radialIndicatorUI.enabled = false; 
        //        myEvent.Invoke();
        //    }
            
            
        //}

        //else
        //{
        //    if (shouldUpdate)
        //    {
        //        indicatorTimer += Time.deltaTime;
        //        radialIndicatorUI.fillAmount = indicatorTimer;

        //        if (indicatorTimer >= maxIndicatorTimer)
        //        {
        //            indicatorTimer = maxIndicatorTimer;
        //            radialIndicatorUI.fillAmount = maxIndicatorTimer;
        //            radialIndicatorUI.enabled = false;
        //            shouldUpdate = false; 
        //        }
        //    }
        //}

        //if (Keyboard.current.gKey.wasReleasedThisFrame)
        //{
        //    radialIndicatorUI.enabled = false; 
        //    shouldUpdate = true;
        //} 
    }

    private IEnumerator StartCountdown(float taskTimer)
    {
        while (!taskComplete && taskTimer > 0f)
        {
            indicatorTimer -= (Time.deltaTime * passengerDelay);
            radialIndicatorUI.fillAmount = indicatorTimer;
            yield return null; 
        }
    } 


}
