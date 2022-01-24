using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractablePickUpMartin : MonoBehaviour, IInteractable
{
    [SerializeField] private float _maxRange = 20;
    public float MaxRange => _maxRange;
    PickUpObjectsMartin pickUpscript;
    Transform player;
    
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pickUpscript = player.GetComponent<PickUpObjectsMartin>();
    }

    public void OnStartHover() {
        //print("Started Hover");
    }

    public void OnInteract() {
        pickUpscript.Interact(gameObject);
    }

    public void OnEndHover() {
        //print("Ended Hover");
    }
}
