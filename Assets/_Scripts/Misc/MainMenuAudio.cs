using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Audio;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class MainMenuAudio : MonoBehaviour
{
   [SerializeField] private Mixtape MainMenuMusic;
   private AudioSource audioSource; 

   private void Awake()
   {
      audioSource = GetComponent<AudioSource>();
   }

   private void Start()
   {
      int trackIndex = Random.Range(0, MainMenuMusic.PlaylistTracks.Length);
      audioSource.clip = MainMenuMusic.PlaylistTracks[trackIndex]; 
      audioSource.Play();
   }
}
