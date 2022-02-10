using UnityEngine;

public class Phone : PhysicsObject
{
    [SerializeField] private LayerMask _raycastOnLayer;
    [SerializeField] private float _colorLerpSpeed;
    
    private Color _color;
    private MeshRenderer _renderer;
    
    private Transform _worldParent;
    private Transform _snapPosition;
    private Vector3 _velocity = Vector3.zero;
    
    private float _chargeAmount;
    
    public bool Charging { get; set; }
    public bool OverHeated { get; private set; }

    public float ChargeAmount
    {
        get => _chargeAmount; 
        set
        {
            _chargeAmount = value;
            if (_chargeAmount >= 10)
            {
                OverHeated = true;
            }
        }
    }
    private void Start()
    {
        GameManager.instance.taskReferences.phones.Add(this);
        
        _renderer = GetComponent<MeshRenderer>();
        _renderer.sharedMaterial = Instantiate(_renderer.sharedMaterial);
        _color = _renderer.sharedMaterial.GetColor("_BaseColor");
        
        _worldParent = transform.parent;
        _chargeAmount = 0;
        
        //Tested value ok result
        _snapSpeed = 10;
    }
    private void OnDestroy()
    {
        GameManager.instance.taskReferences.phones.Remove(this);
    }
    
    private void Update()
    {
        if (Charging)
        {
            LerpColor(_color,Color.green, _colorLerpSpeed);
        }
        else if (Charging && OverHeated)
        {
            LerpColor(_color,Color.red, _colorLerpSpeed * 2);
        }
        
        //For snapping to dock
        if (!beingHeld) return;
        _snapPosition = RayCastForSnap();
        
        if (_snapPosition != null)
        {
            _rb.isKinematic = true;
            
            var tf = transform;
            var stf = _snapPosition;
            var speed = _snapSpeed * Time.deltaTime;
            
            tf.position = Vector3.SmoothDamp(tf.position, stf.position, ref _velocity, speed);
            tf.rotation = Quaternion.Slerp(transform.rotation, _snapPosition.rotation, speed);
            

            // transform.position = Vector3.MoveTowards(transform.position, _snapPosition.position);
            // _rb.MovePosition(transform.position + -direction * _snapSpeed * Time.deltaTime);
            // _rb.MoveRotation(Quaternion.Slerp(transform.rotation, _snapPosition.rotation, _snapSpeed * Time.deltaTime));
        }
        else
        {
            _rb.isKinematic = false;
        }
    }
    
    // public override void FixedUpdate()
    // {
    //     if (!beingHeld) return;
    //     _snapPosition = RayCastForSnap();
    //     
    //     if (_snapPosition != null)
    //     {
    //         _rb.isKinematic = true;
    //         
    //         var direction = (transform.position - _snapPosition.position).normalized;
    //         _rb.MovePosition(transform.position + -direction * _snapSpeed * Time.deltaTime);
    //         
    //         
    //     }
    //     else
    //     {
    //         _rb.isKinematic = false;
    //     }
    //     base.FixedUpdate();
    // }
    
    public void SetToWorldParent()
    {
        transform.parent = _worldParent;
    }
    
    private Transform RayCastForSnap()
    {
        RaycastHit hit;
        
        Ray ray = new Ray(transform.position, -transform.parent.forward);
        
        Debug.DrawRay(transform.position,-transform.parent.forward, Color.red);
        
        if (Physics.Raycast(ray, out hit, 10,_raycastOnLayer))
        {
            if (hit.collider.TryGetComponent(out PhoneDock dock))
            {
                Debug.DrawRay(transform.position,-transform.parent.forward, Color.green);
                
                return dock.transform;
            }
        }
        return null;
    }
    
    private void LerpColor(Color a, Color b, float speed)
    {
        var rend = GetComponent<MeshRenderer>();
        var t = (Mathf.Sin(Time.time * speed) + 1) / 2.0;
        
        if (rend != null)
            rend.sharedMaterial.SetColor("_BaseColor", Color.Lerp(a ,b ,(float)t));
    }
}