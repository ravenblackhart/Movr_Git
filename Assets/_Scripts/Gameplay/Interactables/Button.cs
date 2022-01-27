using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private GameEvent[] _onInteractEvent;
    [SerializeField] private float _maxRange = 10f;
    public float MaxRange => _maxRange;
    public void OnStartHover()
    {
    }

    public void OnInteract()
    {
        for (int i = 0; i < _onInteractEvent.Length; i++)
        {
            _onInteractEvent[i].Raise();
        }
    }

    public void OnEndHover()
    {
    }
}
