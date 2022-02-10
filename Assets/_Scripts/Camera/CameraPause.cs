using Cinemachine;
using UnityEngine;

public class CameraPause : MonoBehaviour
{
    private CinemachineBrain _cmBrain;

    private void Awake()
    {
        _cmBrain = GetComponent<CinemachineBrain>();
    }


    void Update()
    {
        if (_cmBrain != null)
        {
            _cmBrain.enabled = !GameManager.instance.uiManager.pauseLockEnabled;
        }
    }
}
