using UnityEngine;

public class GameEventInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameEvent _gameEvent;
    
    private float _maxRange = 50f;
    public float MaxRange => _maxRange;

    [SerializeField] private CrossHair crossHair; 
    public void OnStartHover()
    {
        crossHair.UpdateCrosshair(this.gameObject);
    }

    public void OnInteract()
    {
        _gameEvent.Raise();
    }

    public void OnEndHover()
    {
        crossHair.ResetCrosshair();
    }
}
