using System;
using System.Collections;
using UnityEngine;

public class PhysicsObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameEvent _onInteractEvent;
    
    [SerializeField] private float _maxRange = 10f;
    [SerializeField] private float _rotationLambda = 6f;
    [SerializeField] private float _snapSpeed = 200f;
    [SerializeField] private float _snapRange = 0.1f;

    public CustomClasses.QueryEvent touchCustomerQueryEvent;
    public UnityEngine.Events.UnityEvent touchCustomerUnityEvent;

    public float MaxRange => _maxRange;
    private bool _beingHeld = false;
    private bool _onSnapTrigger;
    private Transform _snapTarget;
    [SerializeField] private Transform _holdPos;
    private float _snapDistance;
    private float _leaveSnapDistance;
    private Transform _prevParent;
    public bool OnSnapTrigger
    {
        get => _onSnapTrigger;
        set => _onSnapTrigger = value;
    }

    private Rigidbody _rb;

    private void Awake() {
        _rb = gameObject.GetComponent<Rigidbody>();
        _prevParent = transform.parent;
        _holdPos = GameObject.Find("Hold Position").transform;
        if (FindObjectOfType<SnapTrigger>() != null) {
            _snapTarget = FindObjectOfType<SnapTrigger>().transform;
        }
    }

    public void OnStartHover()
    {
        //
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
        //
    }
    
    private void FixedUpdate()
    {
        _leaveSnapDistance = Vector3.Distance(_holdPos.position, transform.position);

        if (_onSnapTrigger)
        {
            transform.parent = _prevParent;
            //transform.position = _snapTarget.position;
            transform.rotation = _snapTarget.rotation;
            Vector3 moveDirection = (_snapTarget.position - transform.position);
            _rb.AddForce(moveDirection * _snapSpeed);

            if (_leaveSnapDistance > _snapRange) {
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Customer"))
        {
            touchCustomerQueryEvent.Invoke(Time.frameCount);

            touchCustomerUnityEvent.Invoke();
        }
    }
}
