using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickUp : MonoBehaviour {
    

    [SerializeField] float moveForce = 250;
    [SerializeField] float pushForce = 100;
    [SerializeField] float drag = 10;
    [SerializeField] float rotateSpeed = 0.5f;
    [SerializeField] float sideThrowForce = 10;

    private CameraSwitcher _cameraSwitcher;
    private PlayerInput _playerInput;
    private InputAction _primaryAction;
    private InputAction _secondaryAction;
    private Vector2 _mouseDelta;
    private Transform _holdPos;
    public bool canMove = true;
    
    private GameObject _gameObjectInHand;
    private PhysicsObject _physicsObjectInHand;
    private Rigidbody _rbInHand;
    
    private Transform _previousParent;

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

        if (_gameObjectInHand != null && !_physicsObjectInHand.OnSnapTrigger) {          
            MoveObject();
        }
        
        //Keeps the object facing the player
        _holdPos.transform.LookAt(transform.GetChild(0).position);
    }
    
    private void OnPrimaryAction(InputAction.CallbackContext context) {
        if (_gameObjectInHand != null && !_physicsObjectInHand.OnSnapTrigger) {
            ThrowObject();
        }

        if (_gameObjectInHand != null && _physicsObjectInHand.GetComponent<Cassette>() != null && _physicsObjectInHand.OnSnapTrigger) {
            _physicsObjectInHand.GetComponent<Cassette>().SlideInCassette();
            _rbInHand = null;
            _gameObjectInHand = null;
            _physicsObjectInHand = null;
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
            _physicsObjectInHand = obj.GetComponent<PhysicsObject>();
            
            _previousParent = obj.transform.parent;
            _rbInHand.useGravity = false;
            _rbInHand.drag = drag;

            _rbInHand.transform.parent = _holdPos;

            StartCoroutine(ShittyFix(obj));
        }
    }

    void MoveObject() 
    {
        
        if (Vector3.Distance(_gameObjectInHand.transform.position, _holdPos.position) > 0.01f) {
            Vector3 moveDirection = (_holdPos.position - _gameObjectInHand.transform.position);
            _rbInHand.AddForce(moveDirection * moveForce);
        }
    }

    public void ThrowObject() 
    {
        _rbInHand.useGravity = true;
        _rbInHand.drag = 0;
        _rbInHand.transform.parent = _previousParent;

        //Need to make it check the cameras movement rather than the delta but kinda works for now
        _rbInHand.AddForce(-_holdPos.forward * pushForce +
                           _holdPos.up * Mathf.Clamp(_mouseDelta.y * sideThrowForce, -200, 200) +
                           -_holdPos.right * Mathf.Clamp(_mouseDelta.x * sideThrowForce, -200, 200));
        _rbInHand = null;
        _gameObjectInHand = null;
        _physicsObjectInHand = null;
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

    IEnumerator ShittyFix(GameObject obj) {
        //yield return new WaitForSeconds(0.1f);
        yield return null;
        yield return null;
        _gameObjectInHand = obj;
    }
}

