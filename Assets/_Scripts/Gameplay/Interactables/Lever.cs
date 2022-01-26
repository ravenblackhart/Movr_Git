using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    // [SerializeField] private GameEvent _onInteractEvent;
    [SerializeField] private float _maxRange = 10f;
    [SerializeField] private float _dragSpeed = 0.01f;

    [Range(0.0f, 10.0f)] 
    [SerializeField] private float _leverValue;
    
    private float _moveDirection;
    private Vector2 _position;
    
    public float MaxRange => _maxRange;
    public float MoveDirection
    { 
        set 
        {
            _moveDirection = value;
            UpdateLeverValue();
        }
    }

    public void OnStartHover()
    {
    }

    public void OnInteract()
    {
        FindObjectOfType<PlayerDragObject>().StartDrag(this);
    }
    
    public void OnEndHover()
    {
    }

    private void UpdateLeverValue()
    {
        _leverValue += _moveDirection * _dragSpeed;
        _leverValue = Mathf.Clamp(_leverValue,0, 10);
        
        UpdateLeverPosition();
    }

    private void UpdateLeverPosition()
    {
        transform.position = new Vector3(_leverValue * _dragSpeed, transform.position.y, transform.position.z);
    }
}
