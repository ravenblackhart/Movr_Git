using System;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    // [SerializeField] private GameEvent _onInteractEvent;
    [SerializeField] protected float _maxRange = 10f;
    [SerializeField] protected float _dragSpeed = 0.01f;

    [Range(0, 1f)] [SerializeField] private protected float _leverValue;

    [SerializeField] protected Transform _orgin;
    [SerializeField] protected Transform _start;
    [SerializeField] protected Transform _end;
    [SerializeField] protected AnimationCurve _curve;    
    
    // public Transform orgin;
    // public Transform start;
    // public Transform end;
    // public AnimationCurve curve;
    
    [SerializeField] protected bool _useY;
    [SerializeField] protected bool _invert;

    protected private PlayerDragObject _playerDrag;
    protected float _moveDirection;

    public bool OnDrag { get; set; }
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

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(start.position, _end.position);
    // }
    
    private void Awake()
    {
        _playerDrag = FindObjectOfType<PlayerDragObject>();
    }

    public virtual void Start()
    {
        _leverValue = 0;
    }

    public void OnStartHover()
    {
        CrossHair.Instance.UpdateCrosshair(gameObject);
    }

    public void OnInteract()
    {
        _playerDrag.StartDrag(this);
    }

    public void OnEndHover()
    {
        CrossHair.Instance.ResetCrosshair();
    }

    public virtual void UpdateLeverValue()
    {
        _leverValue += _moveDirection * Time.deltaTime;
        // _leverValue += _moveDirection * _dragSpeed * Time.deltaTime;
        _leverValue = Mathf.Clamp(_leverValue, 0, 1);
    }

    public virtual void UpdateLeverTransform()
    {
        _orgin.position = Vector3.Lerp(_start.transform.position, _end.transform.position, _curve.Evaluate(_leverValue));
        _orgin.rotation = Quaternion.Slerp(_start.rotation, _end.rotation, _curve.Evaluate(_leverValue));
    }
}
