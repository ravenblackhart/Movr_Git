using UnityEngine;

public class PhysicsObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameEvent _onInteractEvent;
    
    [SerializeField] private float _maxRange = 10f;
    [SerializeField] private float _rotationLambda = 6f;
    public float MaxRange => _maxRange;
    private bool _beingHeld = false;

    private Rigidbody _rb;

    private void Awake() {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

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

    private void FixedUpdate() {
        _rb.angularVelocity = CustomClasses.Damp(_rb.angularVelocity, Vector3.zero, _rotationLambda, Time.fixedDeltaTime);
    }

}
