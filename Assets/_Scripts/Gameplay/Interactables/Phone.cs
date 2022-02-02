using System;
using System.Collections;
using UnityEngine;

public class Phone : PhysicsObject
{
    private float _chargeAmount;
    private bool _touchedCustomer;
    private bool _overHeated;
    
    private bool _onTrigger;
    public bool OnTrigger => _onTrigger;
    
    public bool OverHeated => _overHeated;

    private Transform _worldParent;

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
        _touchedCustomer = false;
        
    }
    private void OnDestroy()
    {
        GameManager.instance.taskReferences.phones.Remove(this);
    }

    public void SetParentToWorld()
    {
        transform.parent = _worldParent;
    }
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Customer"))
    //     {
    //         StartCoroutine(TouchCustomer());
    //     }
    // }
    
    private void CheckValue()
    {
        if (_chargeAmount >= 15)
        {
            _overHeated = true;
        }
    }
    // private IEnumerator TouchCustomer() {
    //     _touchedCustomer = true;
    //     yield return new WaitForSeconds(0.5f);
    //     _touchedCustomer = false;
    //     Destroy(gameObject);
    // }
}