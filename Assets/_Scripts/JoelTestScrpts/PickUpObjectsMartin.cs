using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpObjectsMartin : MonoBehaviour
{
    GameObject heldObject;
    [SerializeField] Transform holdPos;
    [SerializeField] float moveForce = 250;
    [SerializeField] float pushForce = 100;
    [SerializeField] float drag = 10;
    [SerializeField] float rotateSpeed = 0.002f;
    
    Vector3 curPosition;
    Vector3 prevPosition;

    private PlayerInput _playerInput;
    InputAction rightClick;

    float xAxisRotation;
    float yAxisRotation;
    CameraSwitcher cameraSwitcher;
    float oldXRotation;
    float oldYRotation;
    bool rotating;
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        rightClick = _playerInput.actions["SecondaryAction"];
    }

    private void OnEnable()
    {
        rightClick.Enable();
        rightClick.performed += OnSecondaryAction;
    }

    private void OnDisable()
    {
        rightClick.Disable();
        rightClick.performed -= OnSecondaryAction;
    }

    void Start()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Confined;

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
        if (!rotating) {
            oldXRotation = Mouse.current.position.x.ReadValue();
            oldYRotation = Mouse.current.position.y.ReadValue();
        }
        else {
            xAxisRotation = Mouse.current.position.x.ReadValue();
            yAxisRotation = Mouse.current.position.y.ReadValue();
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

        /*objRb.transform.parent = holdPos;*/
        //objRb.transform.parent = physicsParent;
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
    
    
    //If we trigger the input action
    private void OnSecondaryAction(InputAction.CallbackContext context)
    {
        StartCoroutine(RotateUpdateHeldObject());
        
        // if (heldObject != null)
        // {
        //     // rotating = true;
        //     // StartCoroutine(RotateUpdateHeldObject());
        // }
    }
    
    private IEnumerator RotateUpdateHeldObject()
    {
        //Toggle to lock 
        cameraSwitcher.ToggleLock();    
        
        //This loop executes while we're holding the button down
        while (rightClick.ReadValue<float>() != 0)
        {
            //if (heldObject != null)
            //RotateObject();
            yield return null;
        }
        
        //Toggle to unlock
        cameraSwitcher.ToggleLock();   
    }
}
