using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-1)]
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

    // private void Update()
    // {
    //     transform.position = _parent.position;
    //     transform.rotation = _parent.rotation;
    // }

    private IEnumerator LateFixedUpdate()
    {
        yield return _waitForFixedUpdate;
        var tf = transform;
        tf.position = _parent.position;
        tf.rotation = _parent.rotation;
    }
}
