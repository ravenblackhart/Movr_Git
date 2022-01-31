using UnityEngine;

namespace _Scripts.Audio
{
    [CreateAssetMenu(fileName = "Audio Track", menuName = "Scriptable Objects /Audio Track", order = 0)]
    public class Track : ScriptableObject
    {
        public AudioClip TrackAudioClip; 
    }
}