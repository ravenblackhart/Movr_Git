using UnityEngine;
using Cinemachine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _viewCamera;
    [SerializeField] private PlayerView[] _canTransitionFrom;
    
    //Can add range to value? ex: -10, +10
    [SerializeField] private int _transitionAtAxisValue;   
    [SerializeField] private bool _verticalTransition = false;
    //[SerializeField] private InputAction _inputAction;

    private CinemachineBrain _cmBrain;
    private CinemachinePOV _povCamera;
    // private bool _routineRunning;
    // private float _transitionTime;
    // private bool _inTransition;
    private int CameraHorizontalValue { get; set; }
    private int CameraVerticalValue { get; set; }
    
    private void Awake()
    {
        _cmBrain = FindObjectOfType<CinemachineBrain>();
        _viewCamera = GetComponent<CinemachineVirtualCamera>();
        _povCamera = _viewCamera.GetCinemachineComponent<CinemachinePOV>();
        
        // _routineRunning = false;
    }
    
    private void Update()
    {
        CameraHorizontalValue = (int)_povCamera.m_HorizontalAxis.Value;
        CameraVerticalValue = (int)_povCamera.m_VerticalAxis.Value;
        
        if (!_cmBrain.IsBlending && CheckShouldTransition())
        {
            _viewCamera.MoveToTopOfPrioritySubqueue();
            // StartCoroutine(UseThisView());
        }
    }
    
    private bool CheckShouldTransition()
    {
        bool transition = false;
        
        foreach (var view in _canTransitionFrom)
        {
            if (_verticalTransition)
            {
                if (view.CameraVerticalValue == _transitionAtAxisValue)
                {
                    transition = true;
                }
            }
            if (view.CameraHorizontalValue == _transitionAtAxisValue)
            {
                transition = true;
            }
            // Maybe add InputAction support?
        }
        return transition;
    }
    
    // private IEnumerator UseThisView()
    // {
    //     _routineRunning = true;
    //     _viewCamera.MoveToTopOfPrioritySubqueue();
    //     yield return new WaitForSeconds(_transitionTime);
    //     _routineRunning = false;
    // }
}
