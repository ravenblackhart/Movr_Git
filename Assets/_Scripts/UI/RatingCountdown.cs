using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class RatingCountdown : MonoBehaviour
    {
        [SerializeField] private Image ratingDisplay; 
        [SerializeField] [Range(0f, 5f)] private float customerBaseRating = 5f;
        [SerializeField][Range(0f,1f)] private float customerRatingModifier = 0.3f; 
        private float rating;


        private void Update()
        {
            if (Keyboard.current.fKey.isPressed)StartCountdown(customerBaseRating);
        }


        public void StartCountdown(float baseRating)
        {
            StartCoroutine(RatingCountDown(baseRating)); 
        }
        
        private IEnumerator RatingCountDown(float baseRating)
        {
            rating = baseRating; 
            while (rating > 0f)
            {
                ratingDisplay.fillAmount = rating * 0.2f;
                rating -= (Time.deltaTime * customerRatingModifier);
                yield return null; 
            }
        
        }
    }
}