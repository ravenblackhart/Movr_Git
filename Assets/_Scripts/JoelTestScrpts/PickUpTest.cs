using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpTest : MonoBehaviour
{
    GameObject heldObject;
    [SerializeField] Transform holdPos;
    [SerializeField] float moveForce = 250;
    [SerializeField] float pushForce = 100;
    [SerializeField] float drag = 10;
    Vector3 curPosition;
    Vector3 prevPosition;
    InputAction rightClick;

    void Start()
    {

        //PickupObject(cube);
        if (holdPos == null) {
            if (GameObject.Find("Hold Position") != null) {
                holdPos = GameObject.Find("Hold Position").transform;
            }
        }
    }

    void Update()
    {
        if (heldObject != null && Mouse.current.rightButton.isPressed) {
            RotateObject();
        }

    }

    private void FixedUpdate() {
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
        //objRb.constraints = RigidbodyConstraints.FreezeRotation;

        objRb.transform.parent = holdPos;
        heldObject = obj;
    }

    void MoveObject() {
        if (Vector3.Distance(heldObject.transform.position, holdPos.position) > 0.01f) {
            Vector3 moveDirection = (holdPos.position - heldObject.transform.position);
            heldObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }


    }

    public void ThrowObject() {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.drag = 0;
        rb.AddForce(holdPos.position - heldObject.transform.position * -pushForce);
        rb.constraints = RigidbodyConstraints.None;

        rb.transform.parent = null;
        heldObject = null;
    }

    void RotateObject() {
        heldObject.transform.Rotate(Vector3.right * 1f);
    }
}
