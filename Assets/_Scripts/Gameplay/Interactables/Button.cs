using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private float _maxRange = 10f;
    
    public UnityEvent onInteractEvent;
    
    public float MaxRange => _maxRange;
    public void OnStartHover()
    {
        CrossHair.Instance.UpdateCrosshair(gameObject);
    }

    public void OnInteract()
    {
        onInteractEvent.Invoke();
    }

    public void OnEndHover()
    {
        CrossHair.Instance.ResetCrosshair();
    }
}
