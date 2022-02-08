using System;
using UnityEngine;

namespace _Scripts.UI
{
    public class TutorialUI : MonoBehaviour
    {
        private Camera mainCamera;

        private float posX;
        private float posY;
        private float posZ; 
        [SerializeField] private float Offset = 0.02f;
        [SerializeField] private float animSpeed = 5f; 


        private void Awake()
        {
            mainCamera = gameObject.GetComponentInParent<Canvas>().worldCamera; 
            
            posX = transform.localPosition.x;
            posY = transform.localPosition.y;
            posZ = transform.localPosition.z;
        }


        private void Update()
        {
            transform.localPosition = new Vector3(posX, posY + (Mathf.Sin(animSpeed*Time.time) * Offset), posZ);
            Vector3 direction = gameObject.transform.position - mainCamera.transform.position; 
            transform.rotation = Quaternion.LookRotation(direction);
            
            
        }


    }
}