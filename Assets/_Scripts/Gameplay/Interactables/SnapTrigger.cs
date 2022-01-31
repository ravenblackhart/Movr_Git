using UnityEngine;

public class SnapTrigger : MonoBehaviour
{
    [SerializeField] private Transform _snapPosition;
    public Transform SnapPosition => _snapPosition;
    
    
    // private void OnTriggerStay(Collider other) {
    //     if (other.CompareTag("CassetteTrigger")) {
    //         _onTrigger = true;
    //         trigger = other.transform;
    //         _playerPickUp.hoverObj = trigger;
    //         _playerPickUp.hoverDistance = true;
    //     }
    // }
    //
    // private void OnTriggerExit(Collider other) {
    //     if (other.CompareTag("CassetteTrigger")) {
    //         _onTrigger = false;
    //         trigger = null;
    //         _playerPickUp.hoverDistance = false;
    //     }
    // }
}