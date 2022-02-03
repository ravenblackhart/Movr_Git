using UnityEngine;

public class Phone : PhysicsObject
{
    [SerializeField] private LayerMask _raycastOnLayer;
    
    private Transform _worldParent;
    private Transform _snapPosition;

    private float _chargeAmount;

    public bool OverHeated { get; private set; }

    public float ChargeAmount
    {
        get => _chargeAmount; 
        set
        {
            _chargeAmount = value;
            CheckValue();
        }
    }
    private void Start()
    {
        GameManager.instance.taskReferences.phones.Add(this);
        
        _worldParent = transform.parent;
        _chargeAmount = 0;

    }
    private void OnDestroy()
    {
        GameManager.instance.taskReferences.phones.Remove(this);
    }
    
    public override void FixedUpdate()
    {
        if (!beingHeld) return;
        _snapPosition = RayCastForSnap();
        
        if (_snapPosition != null)
        {
            _rb.isKinematic = true;
            
            var direction = (transform.position - _snapPosition.position).normalized;
            _rb.MovePosition(transform.position + -direction * _snapSpeed * Time.deltaTime);
            _rb.MoveRotation(Quaternion.Slerp(transform.rotation, _snapPosition.rotation, _snapSpeed * Time.deltaTime));
        }
        else
        {
            _rb.isKinematic = false;
        }
        base.FixedUpdate();
    }
    
    private void CheckValue()
    {
        if (_chargeAmount >= 10)
        {
            OverHeated = true;
        }
    }
    
    public void SetToWorldParent()
    {
        transform.parent = _worldParent;
    }
    
    private Transform RayCastForSnap()
    {
        RaycastHit hit;
        
        Ray ray = new Ray(transform.position, -transform.parent.forward);
        
        // Debug.DrawRay(transform.position,-transform.parent.forward, Color.red);
        
        if (Physics.Raycast(ray, out hit, 10,_raycastOnLayer))
        {
            if (hit.collider.TryGetComponent(out PhoneDock dock))
            {
                // Debug.DrawRay(transform.position,-transform.parent.forward, Color.green);
                
                return dock.transform;
            }
        }
        return null;
    }
}