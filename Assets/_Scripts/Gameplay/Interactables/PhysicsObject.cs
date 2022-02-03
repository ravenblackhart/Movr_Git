using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour, IInteractable
{
    [SerializeField] private float _maxRange = 10f;
    [SerializeField] private float _rotationLambda = 6f;
    [SerializeField] public float _snapSpeed = 200f;
    [SerializeField] public float _snapRange = 0.1f;
    [SerializeField] public Transform _holdPos;

    private float _snapDistance;
    private float _leaveSnapDistance;
    public bool _onSnapTrigger;

    public CustomClasses.QueryEvent touchCustomerQueryEvent;
    public UnityEngine.Events.UnityEvent touchCustomerUnityEvent;
    
    public float MaxRange => _maxRange;
    public bool beingHeld = false;
    public bool OnSnapTrigger
    {
        get => _onSnapTrigger;
        set => _onSnapTrigger = value;
    }

    public Rigidbody _rb;

    private void Awake() 
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _holdPos = GameObject.Find("Hold Position").transform;
    }
    
    public void OnStartHover()
    {
        //
    }

    public void OnInteract()
    {
        if (!beingHeld)
            FindObjectOfType<PlayerPickUp>().PickupObject(gameObject);
        
        beingHeld = !beingHeld;
    }

    public void OnEndHover()
    {
    }
    
    public virtual void FixedUpdate()
    {
        // if (_onSnapTrigger)
        // {
        //     //Leaving SnapTriggerArea
        //     if (Vector3.Distance(_holdPos.position, transform.position) > _snapRange)
        //     {
        //         transform.parent = _holdPos;
        //         // _onSnapTrigger = false;
        //     }
        // }
        _rb.angularVelocity = CustomClasses.Damp(_rb.angularVelocity, Vector3.zero, _rotationLambda, Time.fixedDeltaTime);
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
