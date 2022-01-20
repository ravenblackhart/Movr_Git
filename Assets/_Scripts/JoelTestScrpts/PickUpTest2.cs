using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTest2 : MonoBehaviour, IInteractable
{
    [SerializeField] private float _maxRange = 20;
    public float MaxRange => _maxRange;
    PickUpTest pickUpTest;
    [SerializeField] Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pickUpTest = player.GetComponent<PickUpTest>();
    }

    public void OnStartHover() {
        //print("Started Hover");
    }

    public void OnInteract() {
        pickUpTest.Interact(gameObject);
    }

    public void OnEndHover() {
        //print("Ended Hover");
    }
}
