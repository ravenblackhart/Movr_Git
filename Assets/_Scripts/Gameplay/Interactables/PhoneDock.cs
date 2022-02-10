using UnityEngine;

public class PhoneDock : MonoBehaviour
{
    private Phone _phone;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Phone phone))
        {
            _phone = phone;
            _phone.OnSnapTrigger = true;
            _phone.Charging = true;
            _phone.transform.rotation = transform.rotation;
            
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (_phone != null)
            if (!_phone.beingHeld)
            {
                //Snap transform
                _phone.transform.position = transform.position;
                _phone.transform.rotation = transform.rotation;
                
                //Charge Phone
                if (!(_phone.ChargeAmount >= 15))
                    _phone.ChargeAmount += Time.deltaTime;
            }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (_phone != null)
        {
            _phone.OnSnapTrigger = false;
            _phone.Charging = false;
            _phone = null;
        }
    }
}