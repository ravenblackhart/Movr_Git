using UnityEngine;

public class SnapTrigger : MonoBehaviour
{
    [SerializeField] private Transform _snapPosition;
    public Transform SnapPosition => _snapPosition;
}