using System;
using UnityEngine;

namespace _Scripts.Audio
{
    [CreateAssetMenu(fileName = "New Mixtape", menuName = "Scriptable Objects /Mixtape", order = 0)]
    public class Mixtape : ScriptableObject
    {
        public String CassetteName ;
        public Musictype Genre; 
        public AudioClip[] PlaylistTracks;

        public enum Musictype
        {
            Jazz, 
            Blues, 
            House, 
            Funk
        }
    }
}