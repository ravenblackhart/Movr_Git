using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRayCast : MonoBehaviour
{
    [SerializeField] private float _castRange;
    [SerializeField] private Transform _eyePos;
    
    
    private Camera _camera;
    private PlayerInput _playerInput;
    private InputAction _interactAction;
    
    private IInteractable _currentTarget;
    private Vector3 _lastCameraDirection;
    
    
    [SerializeField] private LayerMask _raycastOnLayer;
    
    private void Awake()
    {
        _camera = Camera.main;
        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions["PrimaryAction"];
    }

    private void OnEnable()
    {
        _interactAction.Enable();
        _interactAction.performed += OnPrimaryAction;
    }

    private void OnDisable()
    {
        _interactAction.Disable();
        _interactAction.performed -= OnPrimaryAction;
    }
    
    private void Update()
    {
        RayCastCheckInteractable();
    }

    // private void FixedUpdate()
    // {
    //     RayCastCheckInteractable();
    // }
    
    // private void LateUpdate()
    // {
    //     RayCastCheckInteractable();
    // }

    // private void OnPreRender()
    // {
    //     _lastCameraDirection = _camera.transform.forward;
    // }

    private void OnPrimaryAction(InputAction.CallbackContext context)
    {
        if (_currentTarget != null)
        {
            _currentTarget.OnInteract();
        }
    }
    
    private void RayCastCheckInteractable()
    {
        RaycastHit hit;
        // int layerMask = ~_raycastOnLayer;

        // Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width /2, Screen.height / 2, 0));
        
        if (Physics.Raycast(ray, out hit, _castRange))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            // print(hit.transform.name);
            
            if (interactable != null)
            {
                //Return if we cant reach
                if (hit.distance >= interactable.MaxRange) 
                    return;
                
                if (_currentTarget != null)
                {
                    _currentTarget.OnEndHover();
                    
                    _currentTarget = interactable;
                    _currentTarget.OnStartHover();
                }
                else
                {
                    _currentTarget = interactable;
                    _currentTarget.OnStartHover();
                }
            }
            else
                ResetTarget();
        }
        else
            ResetTarget();
    }

    private void ResetTarget()
    {
        if (_currentTarget != null)
        {
            _currentTarget.OnEndHover();
            _currentTarget = null;
        }
    }
}