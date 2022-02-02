using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent _onInteractEvent;
    [SerializeField] private float _maxRange = 10f;
    public float MaxRange => _maxRange;
    public void OnStartHover()
    {
    }

    public void OnInteract()
    {
        _onInteractEvent.Invoke();
    }

    public void OnEndHover()
    {
    }
}
