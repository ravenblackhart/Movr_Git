using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickUp : MonoBehaviour {
    

    [SerializeField] float moveForce = 250;
    [SerializeField] float pushForce = 100;
    [SerializeField] float drag = 10;
    [SerializeField] float rotateSpeed = 0.005f;

    CameraSwitcher cameraSwitcher;
    PlayerInput playerInput;
    InputAction _primaryAction;
    InputAction _secondaryAction;
    Vector2 mouseDelta;
    Transform holdPos;
    
    GameObject heldObject;
    
    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        cameraSwitcher = FindObjectOfType<CameraSwitcher>();
        
        _primaryAction = playerInput.actions["PrimaryAction"];
        _secondaryAction = playerInput.actions["SecondaryAction"];
    }

    private void OnEnable() {
        _primaryAction.Enable();
        _secondaryAction.Enable();
        
        _primaryAction.performed += OnPrimaryAction;
        _secondaryAction.performed += OnSecondaryAction;
    }

    private void OnDisable() {
        _primaryAction.Disable();
        _secondaryAction.Disable();
        
        _primaryAction.performed -= OnPrimaryAction;
        _secondaryAction.performed -= OnSecondaryAction;
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
    
    private void OnPrimaryAction(InputAction.CallbackContext context) {
        if (heldObject != null) {
            ThrowObject();
        }
    }
    private void OnSecondaryAction(InputAction.CallbackContext context) {
        if (heldObject != null) {
            StartCoroutine(RotationUpdate());
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
    
    IEnumerator RotationUpdate() {
        cameraSwitcher.ToggleLock();

        while (_secondaryAction.ReadValue<float>() != 0) {
            RotateObject();
            yield return null;
        }
        cameraSwitcher.ToggleLock();
    }

}

