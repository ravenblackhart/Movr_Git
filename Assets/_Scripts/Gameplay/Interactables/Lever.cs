using System;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    // [SerializeField] private GameEvent _onInteractEvent;
    [SerializeField] private float _maxRange = 10f;
    [SerializeField] private float _dragSpeed = 0.01f;

    private float _currentLeverPosition;
    private float _moveDirection;

    private Vector3 _position;
    private float _min;
    private float _max;
    public float MoveDirection
    {
        get => _moveDirection;
        set 
        {
            _moveDirection = value;
            UpdatePosition();
        }
    }
    public float MaxRange => _maxRange;
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

    private void UpdatePosition()
    {
        _currentLeverPosition += _moveDirection * _dragSpeed;
        _currentLeverPosition = Mathf.Clamp(_currentLeverPosition,-1, 1);
        //
        // transform.position =
        //     Vector3.MoveTowards(transform.position, Vector3.one * _currentLeverPosition, Time.deltaTime);
    }
    
    // private void Update()
    // {
    //     print(_currentLeverPosition);
    // }
}
