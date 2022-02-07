using UnityEngine;
using UnityEngine.Events;

public class EventTriggerAtLeverValue : MonoBehaviour
{
    [SerializeField] private float _triggerAtValue;
    [SerializeField] private UnityEvent _event;
    
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
            _event.Invoke();
    }
}
