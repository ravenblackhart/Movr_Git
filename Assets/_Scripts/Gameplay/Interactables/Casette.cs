using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casette : MonoBehaviour
{
    [SerializeField] public bool _onTrigger = false;
    [SerializeField] float moveForce = 200f;
    public Transform trigger;
    PlayerPickUp _playerPickUp;
    public bool locked = false;
    public bool pickedUp;
    

    void Start()
    {
        _playerPickUp = FindObjectOfType<PlayerPickUp>();
    }

    private void Update() {
        if (_onTrigger) {
            
        }
    }

    private void FixedUpdate() {
        if (locked) {
            if (Vector3.Distance(transform.position, transform.parent.position) > 0.01f) {
                /*Vector3 moveDirection = (transform.parent.position - transform.position);
                transform.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);*/

                transform.position = Vector3.Lerp(transform.position, transform.parent.position, Time.deltaTime * moveForce);
            }

            else {
                transform.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        else {
            transform.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("CassetteTrigger")) {
            _onTrigger = true;
            trigger = other.transform;
            _playerPickUp.hoverObj = trigger;
            _playerPickUp.hoverDistance = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("CassetteTrigger")) {
            _onTrigger = false;
            trigger = null;
            _playerPickUp.hoverDistance = false;
        }
    }
}
