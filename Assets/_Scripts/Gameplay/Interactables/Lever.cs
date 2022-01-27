using System;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    // [SerializeField] private GameEvent _onInteractEvent;
    [SerializeField] private float _maxRange = 10f;
    
    [SerializeField] private float _dragSpeed = 0.01f;
    
    [Range(-1f, 1f)] 
    [SerializeField] private float _leverValue;
    
    [SerializeField] private Transform _orgin;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;

    private float _moveDirection;
    
    public float MaxRange => _maxRange;
    public float MoveDirection
    {
        get => _moveDirection;
        set 
        {
            _moveDirection = value;
            UpdateLeverValue();
        }
    }
    
    public void OnStartHover()
    {
    }

    public void OnInteract()
    {
        FindObjectOfType<PlayerDragObject>().StartDrag(this);
    }
    
    public void OnEndHover()
    {
    }

    private void UpdateLeverValue()
    {
        _leverValue += _moveDirection * _dragSpeed;
        _leverValue = Mathf.Clamp(_leverValue,-1, 1);

        UpdateLeverPosition();
    }

    private void UpdateLeverPosition()
    { 
        _orgin.position = Vector3.Lerp(_startPoint.transform.position, _endPoint.transform.position, _leverValue);
        _orgin.rotation = Quaternion.Lerp(_endPoint.rotation, _startPoint.rotation, _leverValue);
    }
}
