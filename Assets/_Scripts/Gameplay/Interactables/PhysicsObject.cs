using UnityEngine;

public class PhysicsObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameEvent _onInteractEvent;
    
    [SerializeField] private float _maxRange = 10f;
    public float MaxRange => _maxRange;
    public void OnStartHover()
    {
    }

    public void OnInteract()
    {
        FindObjectOfType<PlayerPickUp>().PickupObject(gameObject);
        //        _onInteractEvent.Raise();
    }

    public void OnEndHover()
    {
    }
}
