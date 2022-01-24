using System;
using UnityEngine;

namespace ScriptableEvents
{
    
    public abstract class ScriptableEventBase : ScriptableObject
    {
        private event Action _eventNoPayload;

        public void Register(Action onEventNoPayload)
        {
            _eventNoPayload += onEventNoPayload; 
        }

        public void Unregister(Action onEventNoPayload)
        {
            _eventNoPayload -= onEventNoPayload; 
        }

        public void Raise()
        {
            _eventNoPayload?.Invoke();
        }
    }

    public abstract class ScriptableEvent<T> : ScriptableEventBase
    {
        private event Action<T> _event; 
        
        public void Register(Action<T> onEventPayload)
        {
            _event += onEventPayload; 
        }

        public void Unregister(Action<T> onEventPayload)
        {
            _event -= onEventPayload; 
        }

        public void Raise(T newValue)
        {
            _event?.Invoke(newValue);
        }
    }
}