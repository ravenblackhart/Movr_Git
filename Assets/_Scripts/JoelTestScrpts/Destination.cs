using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    bool arrived = false;
    GameObject zone;
    Transform passengerSpot;
    [SerializeField] GameObject pickupListObj;
    List<Transform> pickupSpots = new List<Transform>();

    void Start()
    {
        zone = transform.Find("Zone").gameObject;
        passengerSpot = transform.Find("Passenger Spot");
        pickupListObj = GameObject.Find("Pickup List");
        foreach (Transform spot in pickupListObj.transform) {
            pickupSpots.Add(spot);
        }
    }

    void Arrive(Transform passenger) {
        arrived = true;
        passenger.SetParent(transform);
        passenger.position = passengerSpot.position;
        passenger.rotation = passengerSpot.rotation;
        zone.SetActive(false);
        ChoosePickupSpot();
    }

    void ChoosePickupSpot() {
        int choice = Random.Range(0, pickupSpots.Count);
        print(choice);
        pickupSpots[choice].gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "CarTemp 2" && !arrived) {
            Arrive(other.transform.Find("Passenger Seat").Find("Passenger"));
        }
    }
}
