using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformToParent : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private Vector3 _origin;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    
    private void Start()
    {
        _origin = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = _origin;
        StartCoroutine(LateFixedUpdate());
    }
    
    private IEnumerator LateFixedUpdate()
    {
        transform.position = _parent.position;
        yield return _waitForFixedUpdate;
    }
}
