using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerAtLeverValue : MonoBehaviour
{
    [SerializeField] private float _triggerAtValue;
    [SerializeField] private string _audioToTrigger;
    
    private Lever _lever;
    private void Awake()
    {
        _lever = GetComponent<Lever>();
    }

    private void Update()
    {
        if(!_lever.OnDrag)
            return;
        
        if (Mathf.Approximately(_lever.LeverValue, _triggerAtValue)) 
            AudioManager.Instance.Play(_audioToTrigger);
    }
}