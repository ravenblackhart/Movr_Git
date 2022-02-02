using System;
using System.Collections;
using UnityEngine;

public class PhoneDock : MonoBehaviour
{
    [SerializeField] private Transform _snapTransform;

    private Phone _phone;
    private Transform _prevParent;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Phone phone))
        {
            _phone = phone;
            _phone.OnSnapTrigger = true;
            _prevParent = phone.transform.parent;
            
            StartCoroutine(LerpToTarget(_phone.transform, _snapTransform.transform, 2));
            
            phone.transform.parent = _snapTransform;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Phone phone))
        {
            phone.ChargeAmount += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _phone.OnSnapTrigger = false;
        _phone = null;
    }

    private IEnumerator LerpToTarget(Transform obj, Transform target, float lerpTime)
    {
        obj.position = Vector3.Lerp(obj.position, target.position, lerpTime);
        yield return null;
    }
    
    //när objektet kommer in i triggern, snapa till transformen
    
}