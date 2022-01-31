using System;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    // [SerializeField] private GameEvent _onInteractEvent;
    [SerializeField] private float _maxRange = 10f;

    [SerializeField] private float _dragSpeed = 0.01f;

    [Range(0, 1f)] [SerializeField] private float _leverValue;

    [SerializeField] private Transform _orgin;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private bool _useY;
    [SerializeField] private bool _invert;

    private PlayerDragObject _playerDrag;
    private float _moveDirection;

    public float MaxRange => _maxRange;
    public float LeverValue => _leverValue;
    public bool UseY => _useY;
    public bool Invert => _invert;
    
    public float MoveDirection
    {
        get => _moveDirection;
        set
        {
            _moveDirection = value;
            UpdateLeverValue();
            UpdateLeverTransform();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_start.position, _end.position);
    }
    
    private void Awake()
    {
        _playerDrag = FindObjectOfType<PlayerDragObject>();
    }

    private void Start()
    {
        _leverValue = 0;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void OnStartHover()
    {
    }

    public void OnInteract()
    {
        _playerDrag.StartDrag(this);
    }

    public void OnEndHover()
    {
    }

    private void UpdateLeverValue()
    {
        _leverValue += _moveDirection * _dragSpeed * Time.deltaTime;
        _leverValue = Mathf.Clamp(_leverValue, 0, 1);
    }

    private void UpdateLeverTransform()
    {
        _orgin.position = Vector3.Lerp(_start.transform.position, _end.transform.position, _curve.Evaluate(_leverValue));
        _orgin.rotation = Quaternion.Slerp(_start.rotation, _end.rotation, _curve.Evaluate(_leverValue));
    }
}
