using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTest : MonoBehaviour
{
    GameObject heldObject;
    [SerializeField] Transform holdPos;
    [SerializeField] float moveForce = 250;
    [SerializeField] float drag = 10;

    void Start()
    {
        //PickupObject(cube);
    }

    void Update()
    {
        if (heldObject != null) {
            MoveObject();
        }
    }

    public void Interact(GameObject obj) {
        if (heldObject == null) {
            PickupObject(obj);
        }

        else {
            ThrowObject();
        }
    }

    public void PickupObject(GameObject obj) {
        Rigidbody objRb = obj.GetComponent<Rigidbody>();
        objRb.useGravity = false;
        objRb.drag = drag;
        objRb.constraints = RigidbodyConstraints.FreezeRotation;

        objRb.transform.parent = holdPos;
        heldObject = obj;
    }

    void MoveObject() {
        if (Vector3.Distance(heldObject.transform.position, holdPos.position) > 0.1f) {
            Vector3 moveDirection = (holdPos.position - heldObject.transform.position);
            heldObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    public void ThrowObject() {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.drag = 0;
        rb.AddForce(rb.velocity);
        rb.constraints = RigidbodyConstraints.None;

        rb.transform.parent = null;
        heldObject = null;
    }
}
