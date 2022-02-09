using System;
using System.Collections;
using UnityEngine;

public class WaterCup : PhysicsObject
{
    private float _fillAmount;
    public float FillAmount => _fillAmount;

    private void Start()
    {
        GameManager.instance.taskReferences.waterCups.Add(this);
    }
    //
    private void OnDestroy()
    {
        GameManager.instance.taskReferences.waterCups.Remove(this);
    }
    
    private void OnParticleCollision(GameObject other)
    {
        if (!(_fillAmount >= 30))
            _fillAmount++;
    }
    
    //TODO: replace method with query-event
    
    private bool _touchedCustomer;
    public bool TouchedCustomer => _touchedCustomer;
    
    private IEnumerator TouchCustomer() {
        _touchedCustomer = true;
        yield return new WaitForSeconds(0.5f);
        _touchedCustomer = false;
        Destroy(gameObject);
    }
}
