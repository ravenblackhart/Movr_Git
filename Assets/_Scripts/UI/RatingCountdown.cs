using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class RatingCountdown : MonoBehaviour
    {
        [SerializeField] private Image[] ratingDisplays;
        private float[] starFallProgs;
        private Vector3[] starDefaultPositions;

        [SerializeField] [Range(0f, 5f)] private float customerBaseRating = 5f;
        [SerializeField] [Range(0f, 1f)] private float customerRatingModifier = 0.3f; 
        private float rating;
        private float progress;

        private void Update()
        {
            //if (Keyboard.current.fKey.isPressed)StartCountdown(customerBaseRating);
        }

        public void UpdateRating(float newRating, bool canFall = true)
        {
            UpdateRatingNoProgress(newRating, canFall);
        }

        public void UpdateRating(float newRating, float progress, bool canFall = true)
        {
            this.progress = progress;

            UpdateRatingNoProgress(newRating, canFall);
        }

        void UpdateRatingNoProgress(float newRating, bool canFall)
        {
            if (starFallProgs == null || starDefaultPositions == null)
            {
                starFallProgs = new float[ratingDisplays.Length];

                starDefaultPositions = new Vector3[ratingDisplays.Length];

                for (int i = 0; i < ratingDisplays.Length; i++)
                {
                    starDefaultPositions[i] = ratingDisplays[i].transform.parent.localPosition;
                }
            }

            for (int i = 0; i < ratingDisplays.Length; i++)
            {
                var fill = newRating - i;

                ratingDisplays[i].fillAmount = fill;

                if (canFall)
                    if (i >= newRating)
                    {
                        starFallProgs[i] += Time.deltaTime * 30f;
                    }
                    else
                    {
                        starFallProgs[i] = 0f;
                    }

                var t = starFallProgs[i];

                UnityEngine.Random.InitState(i);

                float speed = UnityEngine.Random.value;

                float height = UnityEngine.Random.value;

                Vector3 posDeltaFalling =
                    Vector3.up * -t * (t - (0.75f + height * 0.5f) * 13f)
                    + Vector3.right * t * (0.75f + speed * 0.5f) * 2f;
                Vector3 rotDeltaFalling = Vector3.forward * t * 5f;

                Vector3 posDeltaShaking = canFall 
                    ? (Vector3.right * Mathf.Sin(Time.time * 90f) * 2f + Vector3.up * Mathf.Cos(Time.time * 20f)) * Mathf.Clamp01(1f - t / 5f) * Mathf.InverseLerp(0.333f, 0.2f, fill)
                    : Vector3.zero;

                Vector3 posDeltaTransition = Vector3.up * Mathf.Pow(progress - 1f, 2f) * 200f;

                ratingDisplays[i].transform.parent.localPosition = starDefaultPositions[i] + posDeltaFalling + posDeltaShaking + posDeltaTransition;

                ratingDisplays[i].transform.parent.localEulerAngles = rotDeltaFalling;
            }

            rating = newRating;
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
                //ratingDisplay.fillAmount = rating * 0.2f;
                rating -= (Time.deltaTime * customerRatingModifier);
                yield return null; 
            }
        
        }
    }
}