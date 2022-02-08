using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private float _maxRange = 10f;
    [SerializeField] private string _onInteractAudio;
    
    [HideInInspector]public UnityEvent onInteractEvent;
    
    public float MaxRange => _maxRange;
    public void OnStartHover()
    {
        CrossHair.Instance.UpdateCrosshair(gameObject);
    }

    public void OnInteract()
    {
        onInteractEvent.Invoke();
        AudioManager.Instance.Play(_onInteractAudio);
    }

    public void OnEndHover()
    {
        CrossHair.Instance.ResetCrosshair();
    }
}
