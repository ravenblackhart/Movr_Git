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
    
    private IInteractable _currentInteractableTarget;
    private Vector3 _lastCameraDirection;
    
    
    [SerializeField] private LayerMask _ignoreRaycastLayer;
    [SerializeField] private LayerMask _ignoreRaycastLayer2;

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
        RayCastCheck();
        Debug.Log($"{this} hitting interactable: {_currentInteractableTarget}");
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
        if (_currentInteractableTarget != null)
        {
            _currentInteractableTarget.OnInteract();
        }
    }
    
    private void RayCastCheck()
    {
        RaycastHit hit;
        int mask = ~(_ignoreRaycastLayer | _ignoreRaycastLayer2);

        // Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width /2, Screen.height / 2, 0));
        
        if (Physics.Raycast(ray, out hit, _castRange,mask, QueryTriggerInteraction.Ignore))
        {
            // Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            
            //Check for Interactable
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            
            if (interactable != null)
            {
                if (hit.distance >= interactable.MaxRange) 
                    return;
                
                if (_currentInteractableTarget != null)
                {
                    _currentInteractableTarget.OnEndHover();
                    
                    _currentInteractableTarget = interactable;
                    _currentInteractableTarget.OnStartHover();
                }
                else
                {
                    _currentInteractableTarget = interactable;
                    _currentInteractableTarget.OnStartHover();
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
        if (_currentInteractableTarget != null)
        {
            _currentInteractableTarget.OnEndHover();
            _currentInteractableTarget = null;
        }
    }
}