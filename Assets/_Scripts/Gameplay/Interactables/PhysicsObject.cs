using UnityEngine;

public class PhysicsObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameEvent _onInteractEvent;
    
    [SerializeField] private float _maxRange = 10f;
    public float MaxRange => _maxRange;
    private bool _beingHeld = false;
    public void OnStartHover()
    {
    }

    public void OnInteract()
    {
        if (!_beingHeld)
        {
            FindObjectOfType<PlayerPickUp>().PickupObject(gameObject);
        }
        _beingHeld = !_beingHeld;
    }

    public void OnEndHover()
    {
    }
}
