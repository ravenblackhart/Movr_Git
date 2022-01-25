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

    [SerializeField] float xAxisRotation;
    [SerializeField] float yAxisRotation;
    CameraSwitcher cameraSwitcher;
    [SerializeField] float oldXRotation;
    [SerializeField] float oldYRotation;
    bool rotating;

    private PlayerInput _playerInput;
    [SerializeField] Vector2 mouseDelta;
    [SerializeField] Camera mainCamera;


    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        rightClick = _playerInput.actions["SecondaryAction"];
    }

    private void OnEnable() {
        rightClick.Enable();
        rightClick.performed += OnSecondaryAction;
    }

    private void OnDisable() {
        rightClick.Disable();
        rightClick.performed -= OnSecondaryAction;
    }

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
        mouseDelta = _playerInput.actions["MouseLook"].ReadValue<Vector2>();
        //Vector2 projectedMousePos = mainCamera.ScreenToWorldPoint(mousePosition);
        

        if (rotating) {
            RotateObject();

            //xAxisRotation = Mouse.current.position.x.ReadValue();
            //yAxisRotation = Mouse.current.position.y.ReadValue();

            xAxisRotation = mouseDelta.x;
            yAxisRotation = mouseDelta.y;
        }

        else {

            //oldXRotation = Mouse.current.position.x.ReadValue();
            //oldYRotation = Mouse.current.position.y.ReadValue();

            oldXRotation = mouseDelta.x;
            oldYRotation = mouseDelta.y;
        }     
    }

    private void FixedUpdate() {
        if (heldObject != null) {
            MoveObject();           
        }

        holdPos.transform.LookAt(transform.position);
    }

    public void Interact(GameObject obj) {
        if (heldObject == null) {
            PickupObject(obj);
            print("picked up " + obj);
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
        //objRb.transform.parent = physicsParent;
        heldObject = obj;
    }

    void MoveObject() {
        Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
        Rigidbody rb = transform.GetComponentInParent<Rigidbody>();


        if (Vector3.Distance(heldObject.transform.position, holdPos.position) > 0.01f) {
            Vector3 moveDirection = (holdPos.position - heldObject.transform.position);
            heldObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);

            //Can we add car's velocity?
            
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

        heldRb.transform.parent = null;
        heldObject = null;
    }

    void RotateObject() {

        //heldObject.transform.Rotate(Vector3.right, yAxisRotation * rotateSpeed - oldYRotation * rotateSpeed, Space.World);
        //heldObject.transform.Rotate(Vector3.right, mouseDelta.y * rotateSpeed, Space.World);
        //heldObject.transform.Rotate(Vector3.down, mouseDelta.x * rotateSpeed, Space.World);
        //heldObject.transform.Rotate(Vector3.down, xAxisRotation * rotateSpeed - oldXRotation * rotateSpeed, Space.World);

        heldObject.transform.RotateAround(holdPos.position, Vector3.down, mouseDelta.x * rotateSpeed);
        heldObject.transform.RotateAround(holdPos.position, -holdPos.transform.right, mouseDelta.y * rotateSpeed);

    }

    private void OnSecondaryAction(InputAction.CallbackContext context) {
        if (heldObject != null) {
            StartCoroutine(LockCamera());
        }
    }

    IEnumerator LockCamera() {

        cameraSwitcher.ToggleLock();
        rotating = true;

        while (rightClick.ReadValue<float>() != 0) {
            yield return null;
        }

        cameraSwitcher.ToggleLock();
        rotating = false;
    }

}
