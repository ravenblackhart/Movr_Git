using System;
using System.Collections;
using UnityEngine;

public class PhysicsObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameEvent _onInteractEvent;
    
    [SerializeField] private float _maxRange = 10f;
    [SerializeField] private float _rotationLambda = 6f;
    [SerializeField] private float _snapSpeed = 200f;
    
    public float MaxRange => _maxRange;
    private bool _beingHeld = false;
    private bool _onSnapTrigger;
    private Transform _snapTarget;
    [SerializeField] private Transform _holdPos;
    private float _snapDistance;
    private float _leaveSnapDistance;
    public bool OnSnapTrigger
    {
        get => _onSnapTrigger;
        set => _onSnapTrigger = value;
    }

    private Rigidbody _rb;

    private void Awake() {
        _rb = gameObject.GetComponent<Rigidbody>();
        _holdPos = GameObject.Find("Hold Position").transform;
        if (FindObjectOfType<SnapTrigger>() != null) {
            _snapTarget = FindObjectOfType<SnapTrigger>().transform;
        }
    }

    public void OnStartHover()
    {
    }

    public void OnInteract()
    {
        if (!_beingHeld)
        {
            FindObjectOfType<PlayerPickUp>().PickupObject(gameObject);
        }
        _beingHeld = !_beingHeld;
    }

    public void OnEndHover()
    {
    }
    
    private void FixedUpdate()
    {
        _leaveSnapDistance = Vector3.Distance(_holdPos.position, transform.position);

        if (_onSnapTrigger)
        {
            transform.parent = null;
            //transform.position = _snapTarget.position;
            transform.rotation = _snapTarget.rotation;
            Vector3 moveDirection = (_snapTarget.position - transform.position);
            _rb.AddForce(moveDirection * _snapSpeed);

            if (_leaveSnapDistance > 0.2f) {
                _onSnapTrigger = false;
            }
        }
            
        _rb.angularVelocity = CustomClasses.Damp(_rb.angularVelocity, Vector3.zero, _rotationLambda, Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var snapTrigger = other.GetComponent<SnapTrigger>();
        if (snapTrigger != null)
        {
            _snapTarget = snapTrigger.SnapPosition;
            _onSnapTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _onSnapTrigger = false;
        _snapTarget = null;
    }
}
