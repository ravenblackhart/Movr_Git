using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineBrain _cmBrain;
    private CinemachineVirtualCamera _activeCamera;
    private float _shakeTimer;
    public static CinemachineShake Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        _cmBrain = FindObjectOfType<CinemachineBrain>();
    }

    //Used with: CinemachineShake.Instance.ShakeCamera(intensity float, shaketime float);
    public void ShakeCamera(float intensity, float time)
    {
        _activeCamera = _cmBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            _activeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        _shakeTimer = time;
    }
    
    void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    _activeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
