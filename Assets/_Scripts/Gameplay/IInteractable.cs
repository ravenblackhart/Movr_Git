public interface IInteractable
{
    public float MaxRange { get; }

    public void OnStartHover();
    public void OnInteract();
    public void OnEndHover();
}
