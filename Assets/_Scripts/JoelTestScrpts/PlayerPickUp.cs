using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickUp : MonoBehaviour {

    GameObject heldObject;

    [SerializeField] Transform holdPos;
    [SerializeField] float moveForce = 250;
    [SerializeField] float pushForce = 100;
    [SerializeField] float drag = 10;
    [SerializeField] float rotateSpeed = 0.005f;

    InputAction rightClick;
    CameraSwitcher cameraSwitcher;
    PlayerInput playerInput;
    Vector2 mouseDelta;


    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        rightClick = playerInput.actions["SecondaryAction"];
    }

    private void OnEnable() {
        rightClick.Enable();
        rightClick.performed += OnSecondaryAction;
    }

    private void OnDisable() {
        rightClick.Disable();
        rightClick.performed -= OnSecondaryAction;
    }

    void Start() {

        //Temporary 
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

    void Update() {

        //Gets the mouse delta from the Input Controller
        mouseDelta = playerInput.actions["MouseLook"].ReadValue<Vector2>();
    }

    private void FixedUpdate() {

        if (heldObject != null) {
            MoveObject();
        }

        //Keeps the object facing the player
        holdPos.transform.LookAt(transform.position);
    }

    //Called from PhysicsObject
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
        Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
        heldRb.useGravity = true;
        heldRb.drag = 0;
        heldRb.AddForce(holdPos.position - heldObject.transform.position * -pushForce);
        heldRb.constraints = RigidbodyConstraints.None;

        heldRb.transform.parent = null;
        heldObject = null;
    }

    void RotateObject() {
        heldObject.transform.RotateAround(holdPos.position, Vector3.down, mouseDelta.x * rotateSpeed);
        heldObject.transform.RotateAround(holdPos.position, -holdPos.transform.right, mouseDelta.y * rotateSpeed);
    }

    private void OnSecondaryAction(InputAction.CallbackContext context) {
        if (heldObject != null) {
            StartCoroutine(RotationUpdate());
        }
    }

    IEnumerator RotationUpdate() {
        cameraSwitcher.ToggleLock();

        while (rightClick.ReadValue<float>() != 0) {
            RotateObject();
            yield return null;
        }

        cameraSwitcher.ToggleLock();
    }

}

