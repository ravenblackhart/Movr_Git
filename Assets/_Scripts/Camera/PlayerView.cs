using UnityEngine;
using Cinemachine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _viewCamera;
    [SerializeField] private PlayerView[] _canTransitionFrom;
    
    //Can add range to value? ex: -10, +10
    [SerializeField] private int _transitionConditionValue;   
    [SerializeField] private bool _verticalTransition = false;

    private bool inTransition = false;
    
    private CinemachinePOV _povCamera;
    private int CameraHorizontalValue { get; set; }
    private int CameraVerticalValue { get; set; }
    
    
    private void Awake()
    {
        _viewCamera = GetComponent<CinemachineVirtualCamera>();
        _povCamera = _viewCamera.GetCinemachineComponent<CinemachinePOV>();
    }
    
    private void Update()
    {
        if (inTransition)
            return;
        
        CameraHorizontalValue = (int)_povCamera.m_HorizontalAxis.Value;
        CameraVerticalValue = (int)_povCamera.m_VerticalAxis.Value;
        
        //need to add wait to let transition finish
        if (CheckShouldTransition())
        {
            inTransition = true;
            UseThisView();
            inTransition = false;
        }
    }

    private bool CheckShouldTransition()
    {
        bool transition = false;
        
        foreach (var view in _canTransitionFrom)
        {
            if (_verticalTransition)
            {
                if (view.CameraVerticalValue == _transitionConditionValue)
                {
                    transition = true;
                }
            }
            
            if (view.CameraHorizontalValue == _transitionConditionValue)
            {
                transition = true;
            }
        }
        return transition;
    }
    
    private void UseThisView()
    {
        _viewCamera.MoveToTopOfPrioritySubqueue();
    }
}
