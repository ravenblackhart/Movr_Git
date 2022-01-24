using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformToParent : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private Vector3 _orgin;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    
    private void Start()
    {
        _orgin = transform.position;
        StartCoroutine(LateFixedUpdate());
    }

    private void FixedUpdate()
    {
        transform.position = _orgin;
    }
    
    private IEnumerator LateFixedUpdate()
    {
        transform.position = _parent.transform.position;
        yield return _waitForFixedUpdate;
    }
}
