using System;
using UnityEngine;

namespace _Scripts.Audio
{
    [CreateAssetMenu(fileName = "New Cassette", menuName = "Scriptable Objects /Cassettes", order = 0)]
    public class Cassettes : ScriptableObject
    {
        public String CassetteName ; 
        public AudioClip[] TrackAudioClip;
        
    }
}