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
    [SerializeField] float rotateSpeed = 0.002f;
    
    Vector3 curPosition;
    Vector3 prevPosition;
    InputAction rightClick;

    float xAxisRotation;
    float yAxisRotation;
    CameraSwitcher cameraSwitcher;
    float oldXRotation;
    float oldYRotation;
    bool rotating;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        //PickupObject(cube);
        if (holdPos == null) {
            if (GameObject.Find("Hold Position") != null) {
                holdPos = GameObject.Find("Hold Position").transform;
            }
        }

        cameraSwitcher = GameObject.Find("CameraSwitcher").GetComponent<CameraSwitcher>();
    }

    void Update()
    {
        if (heldObject != null && Mouse.current.rightButton.isPressed) {
            RotateObject();
            rotating = true;
        }

        else {
            rotating = false;
        }

        if (!rotating) {
            oldXRotation = Mouse.current.position.x.ReadValue();
            oldYRotation = Mouse.current.position.y.ReadValue();
            cameraSwitcher.UseMeTemporarily(false);
        }

        else {
            xAxisRotation = Mouse.current.position.x.ReadValue();
            yAxisRotation = Mouse.current.position.y.ReadValue();
            cameraSwitcher.UseMeTemporarily(true);
        }

        //worldPos = mainCam.WorldToScreenPoint(Mouse.current.position);
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

        /*objRb.transform.parent = holdPos;*/
        //objRb.transform.parent = physicsParent;
        heldObject = obj;
    }

    void MoveObject() {
        Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
        Rigidbody rb = transform.GetComponentInParent<Rigidbody>();

        if (Vector3.Distance(heldObject.transform.position, holdPos.position) > 0.01f) {
            Vector3 moveDirection = (holdPos.position - heldObject.transform.position);
            heldObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);

            // Can we add car's velocity?
            
            //heldRb.velocity = rb.velocity + moveDirection * moveForce;
        }

        else {
            //heldRb.velocity = rb.velocity;
        }

    }

    public void ThrowObject() {
        Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
        Rigidbody rb = transform.GetComponentInParent<Rigidbody>();
        heldRb.useGravity = true;
        heldRb.drag = 0;
        heldRb.AddForce(holdPos.position - heldObject.transform.position * -pushForce);
        heldRb.constraints = RigidbodyConstraints.None;

        /*heldRb.transform.parent = null;*/
        heldObject = null;
    }

    void RotateObject() {        
        heldObject.transform.Rotate(Vector3.right, yAxisRotation * rotateSpeed - oldYRotation * rotateSpeed, Space.World);
        heldObject.transform.Rotate(Vector3.down, xAxisRotation * rotateSpeed - oldXRotation * rotateSpeed, Space.World);
    }
}
