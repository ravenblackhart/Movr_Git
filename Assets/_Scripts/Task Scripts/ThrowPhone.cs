using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPhone : MonoBehaviour
{
    [SerializeField] private float _activateCollisionAfter;
    [SerializeField] private Vector3 _throwOrginOffset;
    
    
    [SerializeField] private float _throwForceForward = 300;
    [SerializeField] private float _throwForceUp = 150;
    [SerializeField] private float _randomRange = 100;
    public float throwTimer = 2;

    public void Throw(GameObject phone) {
        GameObject thrownPhone = Instantiate(phone, transform);
        thrownPhone.transform.position = transform.position + _throwOrginOffset;
        thrownPhone.transform.rotation = transform.rotation;
        thrownPhone.GetComponent<BoxCollider>().isTrigger = true;
        thrownPhone.GetComponent<Rigidbody>().AddForce(transform.forward * _throwForceForward + Vector3.up * _throwForceUp + 
            transform.right * Random.Range(-_randomRange, _randomRange));
        StartCoroutine(TriggerOff(thrownPhone));
    }

    IEnumerator TriggerOff(GameObject phone) {
        yield return new WaitForSeconds(_activateCollisionAfter);
        phone.GetComponent<BoxCollider>().isTrigger = false;
    }
}
