using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cameraFront;
    [SerializeField] private CinemachineVirtualCamera _cameraBack;
    [SerializeField] private CinemachineVirtualCamera _cameraLocked;
    
    private CinemachineBrain _cmBrain;
    private CinemachinePOV _activePov;
    private View _currentView;
    private View _previousView;
    
    //Tillfälligt
    private CinemachineVirtualCamera _previousCamera;
    
    private bool _lockedView = false;

    private enum View
    {
        Front,
        Back,
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
            case View.Front :
                //Front to back transition condition
                if (_activePov != null)
                    if (_activePov.m_HorizontalAxis.Value >= _activePov.m_HorizontalAxis.m_MaxValue)
                    {
                        _currentView = View.Back;
                        return true;
                    }
                break;
            
            case View.Back :
                //Back to front transition condition
                if (_activePov != null)
                    if (_activePov.m_HorizontalAxis.Value <= _activePov.m_HorizontalAxis.m_MinValue)
                    {
                        _currentView = View.Front;
                        return true;
                    }
                break;
            
            case View.Locked :
                if (!_lockedView)
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
                
                case View.Back :
                    vCam = _cameraBack;
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

    public void UseMeTemporarily(bool sendTrueIfYouWantLockedView )
    {
        if (sendTrueIfYouWantLockedView)
        {
            _previousCamera = _cmBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
            _cameraLocked.MoveToTopOfPrioritySubqueue();
        }
        else
        {
            if (_previousCamera != null)
                _previousCamera.MoveToTopOfPrioritySubqueue();
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
            _currentView = _previousView;
        
        _lockedView = !_lockedView;
    }
}
