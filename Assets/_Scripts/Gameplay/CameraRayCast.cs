using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRayCast : MonoBehaviour
{
    [SerializeField] private float _castRange;
    
    private Camera _camera;
    private PlayerInput _playerInput;
    private InputAction _interactAction;
    
    private IInteractable _currentTarget;
    
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

              
        // Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        
        if (Physics.Raycast(ray, out hit, _castRange))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

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