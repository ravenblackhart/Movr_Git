using UnityEngine;

public class PhoneDock : MonoBehaviour
{
    [SerializeField] private Transform _snapTransform;
    [SerializeField] private float  _snapSpeed;

    private Phone _phone;
    private Rigidbody _rb;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Phone phone))
        {
            _phone = phone;
            _rb = _phone.GetComponent<Rigidbody>();
            _phone.OnSnapTrigger = true;
            _phone.transform.parent = _snapTransform;
        }
    }

    private void FixedUpdate()
    {
        if (_phone == null) return;
        if (_phone.OnSnapTrigger) 
        {
            _phone.transform.rotation = transform.rotation;
            Vector3 moveDirection = (_snapTransform.position - _phone.transform.position).normalized;
            
            _rb.MovePosition(_snapTransform.position + moveDirection * _snapSpeed);        
        }
    }
    
    private void Update()
    {
        if (_phone == null) return;
        if (_phone.OnTrigger)
        {
            _phone.ChargeAmount += Time.deltaTime;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (_phone != null)
        {
            _phone.OnSnapTrigger = false;
            _phone = null;
        }
    }
    
}