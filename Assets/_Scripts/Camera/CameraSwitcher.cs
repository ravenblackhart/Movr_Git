using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cameraFront;
    [SerializeField] private CinemachineVirtualCamera _cameraSteering;
    [SerializeField] private CinemachineVirtualCamera _cameraLocked;
    
    private CinemachineBrain _cmBrain;
    private CinemachinePOV _activePov;
    private View _currentView;
    private View _previousView;
    private bool _lockedView = false;
    private bool _steeringView = false;
    
    private enum View
    {
        Front,
        Steering,
        Locked
    }
    
    private void Awake()
    {
        _cmBrain = FindObjectOfType<CinemachineBrain>();
    }
    
    private void Start()
    {
        _currentView = View.Front;
        SetCamera();
    }

    private void Update()
    {
        if (!CheckShouldSwitch())
            return;
        SetCamera();
    }

    private bool CheckShouldSwitch()
    {
        switch (_currentView)
        {
            // case View.Front :
            //     //Front to back transition condition
            //     if (_activePov != null)
            //         if (_activePov.m_HorizontalAxis.Value >= _activePov.m_HorizontalAxis.m_MaxValue)
            //         {
            //             _currentView = View.Back;
            //             return true;
            //         }
            //     break;
            //
            case View.Locked :
                if (_lockedView)
                {
                    return true;
                }
                break;
            
            case View.Steering :
                if (_steeringView)
                {
                    return true;
                }
                break;
        }
        return false;
    }

    private void SetCamera()
    {
        if (!_cmBrain.IsBlending)
        {
            CinemachineVirtualCamera vCam;
            
            switch (_currentView)
            {
                case View.Front :
                    vCam = _cameraFront;
                    break;
                
                case View.Steering :
                    vCam = _cameraSteering;
                    break;
                
                case View.Locked :
                    vCam = _cameraLocked;
                    break;
                
                default:
                    vCam = _cameraFront;
                    break;
            }
            
            vCam.MoveToTopOfPrioritySubqueue();
            _activePov = vCam.GetCinemachineComponent<CinemachinePOV>();
        }
    }
    
    public void ToggleLock()
    {
        if (!_lockedView)
        {
            _previousView = _currentView;
            _currentView = View.Locked;
        }
        else
        {
            _currentView = _previousView;
            SetCamera();
        }
        _lockedView = !_lockedView;
    }

    public void ToggleSteering()
    {
        if (!_steeringView)
        {
            _previousView = _currentView;
            _currentView = View.Steering;
        }
        else
        {
            _currentView = _previousView;
            SetCamera();
        }
        _steeringView = !_steeringView;
    }
}
