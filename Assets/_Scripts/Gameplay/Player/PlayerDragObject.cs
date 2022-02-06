using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDragObject : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _primaryAction;
    private InputAction _secondaryAction;

    private Lever _clickedLever;
    private Vector2 _mouseDelta;
    private CameraSwitcher _cameraSwitcher;
    
    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        _primaryAction = _playerInput.actions["PrimaryAction"];
        _cameraSwitcher = FindObjectOfType<CameraSwitcher>();

        Cursor.lockState = CursorLockMode.Confined;
    }
    
    private void OnEnable() {
        _primaryAction.Enable();
    }
    
    private void OnDisable() {
        _primaryAction.Disable();
    }
    
    public void StartDrag(Lever lever)
    {
        _clickedLever = lever;
        StartCoroutine(DragUpdate());
    }

    private IEnumerator DragUpdate()
    {
        _clickedLever.OnDrag = true;
        _cameraSwitcher.ToggleLock();
        
        while (_primaryAction.ReadValue<float>() != 0)
        {
            DragObject();
            yield return null;
        }
        _clickedLever.OnDrag = false;
        _cameraSwitcher.ToggleLock();
    }

    private void DragObject()
    {
        // _mouseDelta = _playerInput.actions["MouseLook"].ReadValue<Vector2>().normalized;
        _mouseDelta = _playerInput.actions["MouseLook"].ReadValue<Vector2>();
        _mouseDelta= !_clickedLever.Invert ? _mouseDelta : -_mouseDelta;
        _clickedLever.MoveDirection = !_clickedLever.UseY ? _mouseDelta.x : _mouseDelta.y;
    }
}
