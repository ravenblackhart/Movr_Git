using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
  public Transform Player;
  [SerializeField] private Transform driverMarker; 

  private void Update()
  {
    driverMarker.LookRotation(GameObject.FindGameObjectWithTag("PassengerDropoff").transform); 
  }

  private void LateUpdate()
  {
    Vector3 newPosition = Player.position;
    newPosition.y = transform.position.y;
    transform.position = newPosition;

    transform.rotation = Quaternion.Euler(90f, Player.eulerAngles.y, 0f); 

  }
}
