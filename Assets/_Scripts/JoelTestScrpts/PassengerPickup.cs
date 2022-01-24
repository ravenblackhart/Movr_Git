using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerPickup : MonoBehaviour
{
    bool pickedUp = false;
    Transform passenger;
    GameObject zone;
    Transform passengerSeat;
    [SerializeField] GameObject destinationsObj;
    [SerializeField] List<Transform> destinations = new List<Transform>();

    void Start()
    {
        passenger = transform.Find("Passenger");
        zone = transform.Find("Zone").gameObject;
        destinationsObj = GameObject.Find("Destination List");
        foreach (Transform destination in destinationsObj.transform) {
            destinations.Add(destination);
        }
    }

    void PickUpPassenger(Transform seat)
    {
        pickedUp = true;        
        ChooseDestination();
        passenger.SetParent(seat);
        passenger.position = seat.position;
        passenger.rotation = seat.rotation;
        passenger.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        zone.SetActive(false);
    }

    void ChooseDestination() {
        int choice = Random.Range(0, destinations.Count);
        print(choice);
        destinations[choice].gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "CarTemp 2" && !pickedUp) {
            PickUpPassenger(other.transform.Find("Passenger Seat"));
        }
    }
}
