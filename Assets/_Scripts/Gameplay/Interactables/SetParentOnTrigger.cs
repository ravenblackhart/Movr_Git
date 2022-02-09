using System;
using UnityEngine;

public class SetParentOnTrigger : MonoBehaviour
{

    [SerializeField] private Transform _parent;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PhysicsObject obj))
        {
            obj.transform.parent = _parent.transform;
        }
    }
}
