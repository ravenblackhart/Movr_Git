using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickUp : MonoBehaviour {
    

    [SerializeField] float moveForce = 250;
    [SerializeField] float pushForce = 100;
    [SerializeField] float drag = 10;
    [SerializeField] float rotateSpeed = 0.005f;
    [SerializeField] float sideThrowForce = 10;

    CameraSwitcher _cameraSwitcher;
    PlayerInput _playerInput;
    InputAction _primaryAction;
    InputAction _secondaryAction;
    Vector2 _mouseDelta;
    Transform _holdPos;
    
    GameObject _gameObjectInHand;
    private Rigidbody _rbInHand;
    
    Transform _previousParent;

    public bool hover = false;
    public bool hoverDistance = true;
    public Transform hoverObj;

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

        if (hover) {
            Hover(hoverObj);
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

            if (obj.GetComponent<Casette>() != null && obj.GetComponent<Casette>().locked) {
                obj.GetComponent<Casette>().locked = false;
            }

            _previousParent = obj.transform.parent;
            _rbInHand.useGravity = false;
            _rbInHand.drag = drag;

            _rbInHand.transform.parent = _holdPos;

            StartCoroutine(ShittyFix(obj));
        }
    }

    void MoveObject() {
        if (hoverDistance) {
            if (Vector3.Distance(hoverObj.transform.position, _holdPos.position) > 0.2f) {
                _gameObjectInHand.transform.parent = _holdPos;
                hover = false;
            }

            else {
                hover = true;
            }
        }

        if (!hover) {
            if (Vector3.Distance(_gameObjectInHand.transform.position, _holdPos.position) > 0.01f) {
                Vector3 moveDirection = (_holdPos.position - _gameObjectInHand.transform.position);
                _rbInHand.AddForce(moveDirection * moveForce);
            }
        }
    }

    public void ThrowObject() {
        if (hover) {
            /*_rbInHand.useGravity = true;
            _rbInHand.drag = 0;
            _rbInHand.transform.parent = _previousParent;

            _rbInHand = null;
            _gameObjectInHand = null;
            hover = false;*/

            _gameObjectInHand.GetComponent<Casette>().locked = true;
            _rbInHand.transform.parent = hoverObj.GetChild(0);
            _gameObjectInHand = null;
            _rbInHand = null;
            hover = false;
        }

        else {
            _rbInHand.useGravity = true;
            _rbInHand.drag = 0;
            _rbInHand.transform.parent = _previousParent;

            //Need to make it check the cameras movement rather than the delta but kinda works for now
            _rbInHand.AddForce(-_holdPos.forward * pushForce +
                _holdPos.up * Mathf.Clamp(_mouseDelta.y * sideThrowForce, -200, 200) +
                -_holdPos.right * Mathf.Clamp(_mouseDelta.x * sideThrowForce, -200, 200));

            _rbInHand = null;
            _gameObjectInHand = null;
        }
        
    }

    public void Hover(Transform target) {
        hover = true;
        _gameObjectInHand.transform.parent = null;

        if (Vector3.Distance(_gameObjectInHand.transform.position, target.position) > 0.01f) {
            Vector3 moveDirection = (target.position - _gameObjectInHand.transform.position);
            _rbInHand.AddForce(moveDirection * moveForce);
        }

        //_gameObjectInHand.transform.Rotate(target.transform.rotation.eulerAngles * Time.deltaTime);
        if (Vector3.Distance(_gameObjectInHand.transform.rotation.eulerAngles, target.rotation.eulerAngles) > 0.01f) {
            _gameObjectInHand.transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, target.rotation.eulerAngles, Time.deltaTime * 0.00001f);
        }
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

