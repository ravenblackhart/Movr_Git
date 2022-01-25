using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private GameEvent _onInteractEvent;
    [SerializeField] private float _maxRange = 10f;
    public float MaxRange => _maxRange;
    public void OnStartHover()
    {
    }

    public void OnInteract()
    {
        //_onInteractEvent.Raise();
    }

    public void OnEndHover()
    {
    }
}
