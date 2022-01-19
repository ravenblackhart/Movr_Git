using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private float _maxRange = 20; 
    public float MaxRange => _maxRange;
    
    public void OnStartHover()
    {
        print("Started Hover");
    }

    public void OnInteract()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }

    public void OnEndHover()
    {
        print("Ended Hover");
    }
}
