using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
  public Transform Player;
  [SerializeField] private Image driverMarker;
  [SerializeField] private Color32 activeWaypointColor;
  [SerializeField] private float cameraModifier = 90f; 
  [SerializeField] private float markerModifier = 0f;
  
  
  [CanBeNull] private Transform targetLocation;
  [CanBeNull] private Transform targetCanvas;

  

  private void Update()
  {
    if (GameManager.instance.inRide == true) targetLocation = GameObject.FindGameObjectWithTag("PassengerDropoff").transform;
    else if (GameManager.instance.inRide == false) targetLocation = GameObject.FindGameObjectWithTag("PassengerPickup").transform;
    else if (GameManager.instance.inRide == false && GameObject.FindGameObjectWithTag("PassengerPickup").transform == null) targetLocation = null;

    if (targetLocation != null)
    {
      //driverMarker.color = activeWaypointColor;
      
    }
    
    else if (targetLocation == null)
    {
      driverMarker.color = Color.magenta;
      driverMarker.transform.rotation = Quaternion.Euler(90f, markerModifier, 90f ); 
    }
  }

  private void LateUpdate()
  {
    Vector3 newPosition = Player.position;
    newPosition.y = transform.position.y;
    transform.position = newPosition;
    
    PointAt(targetLocation);
    transform.rotation = Quaternion.Euler(90f, Player.eulerAngles.y , cameraModifier);
  }

  private void PointAt(Transform target)
  {
    var pos = transform.position;
    var dir = target.position - pos;
    var rotation = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    driverMarker.transform.rotation = Quaternion.Euler(90f, 0f, rotation - markerModifier );

  }
}
