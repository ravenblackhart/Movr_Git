using System;
using UnityEngine;

    [CreateAssetMenu(fileName = "new GameEvent", menuName = "ScriptableObjects/Events/GameEvent", order = 0)]
    public class GameEvent : ScriptableObject
    {

        private event Action _event;

        public void Raise()
        {
            _event?.Invoke();
        }
        
        public void Register(Action onEvent)
        {
            _event += onEvent;
        }
        
        public void Unregister(Action onEvent)
        {
            _event -= onEvent;
        }
    }

