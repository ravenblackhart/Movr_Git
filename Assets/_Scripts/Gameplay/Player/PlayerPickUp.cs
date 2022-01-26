using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickUp : MonoBehaviour {
    

    [SerializeField] float moveForce = 250;
    [SerializeField] float pushForce = 100;
    [SerializeField] float drag = 10;
    [SerializeField] float rotateSpeed = 0.005f;

    CameraSwitcher _cameraSwitcher;
    PlayerInput _playerInput;
    InputAction _primaryAction;
    InputAction _secondaryAction;
    Vector2 _mouseDelta;
    Transform _holdPos;
    
    GameObject _gameObjectInHand;
    private Rigidbody _rbInHand;
    
    Transform _previousParent;
    
    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        _cameraSwitcher = FindObjectOfType<CameraSwitcher>();
        
        _primaryAction = _playerInput.actions["PrimaryAction"];
        _secondaryAction = _playerInput.actions["SecondaryAction"];
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
        if (_holdPos == null) {
            if (GameObject.Find("Hold Position") != null) {
                _holdPos = GameObject.Find("Hold Position").transform;
            }
        }
    }

    void Update() {

        //Gets the mouse delta from the Input Controller
        _mouseDelta = _playerInput.actions["MouseLook"].ReadValue<Vector2>();
    }

    private void FixedUpdate() {

        if (_gameObjectInHand != null) {
            MoveObject();
        }

        //Keeps the object facing the player
        _holdPos.transform.LookAt(transform.position);
    }
    
    private void OnPrimaryAction(InputAction.CallbackContext context) {
        if (_gameObjectInHand != null) {
            ThrowObject();
        }
    }
    
    private void OnSecondaryAction(InputAction.CallbackContext context) {
        if (_gameObjectInHand != null) {
            StartCoroutine(RotationUpdate());
        }
    }
    
    public void PickupObject(GameObject obj) {

        if (_gameObjectInHand == null)
        { 
            _rbInHand = obj.GetComponent<Rigidbody>();
            _previousParent = obj.transform.parent;
        
            _rbInHand.useGravity = false;
            _rbInHand.drag = drag;

            _rbInHand.transform.parent = _holdPos;
        
            _gameObjectInHand = obj;
        }
    }

    void MoveObject() {
        if (Vector3.Distance(_gameObjectInHand.transform.position, _holdPos.position) > 0.01f) {
            Vector3 moveDirection = (_holdPos.position - _gameObjectInHand.transform.position);
            _rbInHand.AddForce(moveDirection * moveForce);
        }
    }

    public void ThrowObject() {

        _rbInHand.useGravity = true;
        _rbInHand.drag = 0;

        _rbInHand.transform.parent = _previousParent;

        _rbInHand = null;
        _gameObjectInHand = null;
    }

    void RotateObject() {
        _gameObjectInHand.transform.RotateAround(_holdPos.position, Vector3.down, _mouseDelta.x * rotateSpeed);
        _gameObjectInHand.transform.RotateAround(_holdPos.position, -_holdPos.transform.right, _mouseDelta.y * rotateSpeed);
    }
    
    IEnumerator RotationUpdate() {
        _cameraSwitcher.ToggleLock();

        while (_secondaryAction.ReadValue<float>() != 0) {
            RotateObject();
            yield return null;
        }
        _cameraSwitcher.ToggleLock();
    }

}

