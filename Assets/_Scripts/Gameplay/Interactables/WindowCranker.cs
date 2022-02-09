using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WindowCranker : MonoBehaviour
{
    private Camera _mainCamera;

    private CameraSwitcher _cameraSwitcher;

    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    private PlayerInput _playerInput;
    private InputAction _mouseClick;
    private Vector2 _mouseDelta;
    private Vector2 _prevDelta;

    [SerializeField]
    private float _rotateSpeed = 120;
    private float _zAngle = -90;

    private readonly int _maxCrankAngle = 65;

    private string _windowTag = "Window";
    private GameObject _windows;

    private float _yPosWindows;
    private readonly float MAX_Y_POS = 0.85f;
    private readonly float MIN_Y_POS = 0.31f;

    private float _leverValue = 1;
    public float LeverValue => _leverValue;

    private readonly string _windowCrank = "WindowCrank";
    private readonly float _windowSpeed = 0.35f; 

    private Sound _crankingSound;

    private void Awake()
    {
        _windows = GameObject.FindGameObjectWithTag(_windowTag);
        _yPosWindows = MAX_Y_POS;
        _playerInput = FindObjectOfType<PlayerInput>();
        _cameraSwitcher = FindObjectOfType<CameraSwitcher>();
        _mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Confined;

        _mouseClick = _playerInput.actions["PrimaryAction"];
        _zAngle = -_maxCrankAngle;

        
        //AudioManager.Instance.Play(_windowCrank);
        //_crankingSound.source.Pause();
    }

    void Start()
    {
        _crankingSound = AudioManager.Instance.GetSound(_windowCrank);

    }

    void Update()
    {
        if (_crankingSound == null)
        {
            Debug.LogWarning("Höpp");
            _crankingSound = FindCrankingSound(_windowCrank);
        }
        // Find mouse movement
        _mouseDelta = _playerInput.actions["MouseLook"].ReadValue<Vector2>().normalized;
    }

    private void OnEnable()
    {
        _mouseClick.Enable();
        _mouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        _mouseClick.performed -= MousePressed;
        _mouseClick.Disable();
    }

    private Sound FindCrankingSound(string name)
    {
        return Array.Find(AudioManager.Instance.sounds, sound => sound.name == name);
    }

    private void MousePressed(InputAction.CallbackContext context)
    {
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = _mainCamera.ScreenPointToRay(center);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject.tag == _windowCrank)
            {
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }
    }

    private IEnumerator DragUpdate(GameObject crank)
    {
        _cameraSwitcher.ToggleLock();
        // Start sound
        _crankingSound.source.Play();
        while (_mouseClick.ReadValue<float>() != 0)
        {
            // Check mouse movement along y-axis
            if (_mouseDelta.y != 0)
            {
                _prevDelta = _mouseDelta;
            }

            _zAngle -= _prevDelta.y * _rotateSpeed * Time.deltaTime;
            
            _zAngle = Mathf.Clamp(_zAngle, -_maxCrankAngle, _maxCrankAngle);
            // Rotate crank 
            crank.transform.localEulerAngles = new Vector3(crank.transform.localEulerAngles.x,
                crank.transform.localEulerAngles.y, _zAngle);
            // Raise/lower windows
            _yPosWindows = Mathf.Clamp(_yPosWindows += 
                (Time.deltaTime * _windowSpeed * _prevDelta.y), MIN_Y_POS, MAX_Y_POS);
            _leverValue = ChangeLeverValue(_yPosWindows);
            _windows.transform.localPosition = new Vector3(_windows.transform.localPosition.x,
                _yPosWindows, _windows.transform.localPosition.z); 

            yield return _waitForFixedUpdate;
        }
        // Stop this soundly madness
        _crankingSound.source.Stop();
        // Resume to the normal camera
        _cameraSwitcher.ToggleLock();
    }

    private float ChangeLeverValue(float yPos)
    {
        // Resume sound...
        _crankingSound.source.UnPause();
        // ... unless:
        if (yPos <= MIN_Y_POS + 0.05f)
        {
            // Pause sound
            _crankingSound.source.Pause();
            return 0;
        }
        else if (yPos >= MAX_Y_POS - 0.05f)
        {
            // Pause sound
            _crankingSound.source.Pause();
            return 1;
        }
        else
        {
            return _leverValue;
        }
    }
}
