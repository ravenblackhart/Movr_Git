using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDragObject : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction _primaryAction;
    private InputAction _secondaryAction;
    private Camera _mainCamera;
    
    private Vector2 _mouseDelta;

    
    private Lever _clickedObject;
    
    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        _primaryAction = playerInput.actions["PrimaryAction"];
        _mainCamera = Camera.main;
    }

    private void OnEnable() {
        _primaryAction.Enable();
    }
    
    private void OnDisable() {
        _primaryAction.Disable();
    }
    
    public void StartDrag(Lever lever)
    {
        _clickedObject = lever;
        StartCoroutine(DragUpdate());
    }

    private IEnumerator DragUpdate()
    {
        while (_primaryAction.ReadValue<float>() != 0)
        {
            DragObject();
            yield return null;
        }
    }

    private void DragObject()
    {
        _mouseDelta = playerInput.actions["MouseLook"].ReadValue<Vector2>().normalized;
        _clickedObject.MoveDirection += _mouseDelta.x;
    }
}
