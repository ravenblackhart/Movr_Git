using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
  public Transform Player;
  [SerializeField] private Transform driverMarker;
  [SerializeField] private Color32 activeWaypointColor;
  [SerializeField] private float cameraModifier = 90f; 
  [SerializeField] private float markerModifier = 0f;
  
  [CanBeNull] private Transform targetLocation;

  

  private void Update()
  {
    if (GameManager.instance.currentCustomer == null) targetLocation = GameObject.FindGameObjectWithTag("PassengerPickup").transform;
    else if (GameManager.instance.currentCustomer != null) targetLocation = GameObject.FindGameObjectWithTag("PassengerDropoff").transform;

    if (targetLocation != null)
    {
      driverMarker.GetComponent<Image>().color = activeWaypointColor; 
    }
    
    else if (targetLocation == null)
    {
      driverMarker.GetComponent<Image>().color = Color.white;
    }
  }

  private void LateUpdate()
  {
    Vector3 newPosition = Player.position;
    newPosition.y = transform.position.y;
    transform.position = newPosition;

    PointAt(targetLocation);
    transform.rotation = Quaternion.Euler(90f, Player.eulerAngles.y + cameraModifier , 0f); 

  }

  private void PointAt(Transform target)
  {
    var pos = transform.position;
    var dir = target.position - pos;
    var rotation = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    driverMarker.transform.rotation = Quaternion.Euler(90f, Player.eulerAngles.y + markerModifier, rotation - 90f );   
      ; 
  }
}
