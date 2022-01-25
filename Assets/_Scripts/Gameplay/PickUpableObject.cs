using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpableObject : MonoBehaviour, IInteractable {

    [SerializeField] private float _maxRange = 20;
    public float MaxRange => _maxRange;
    PickUpTest pickUpTest;
    [SerializeField] Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pickUpTest = player.GetComponent<PickUpTest>();
    }

    public void OnStartHover() {
        
    }

    public void OnInteract() {
        pickUpTest.Interact(gameObject);
    }

    public void OnEndHover() {
       
    }
}
