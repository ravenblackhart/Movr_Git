using UnityEngine;

namespace _Scripts.ScriptableVariables
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "new FloatVariable", menuName="ScriptableVariables/FloatVariable")]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField] private float _floatValue;

        private float _currentFloatValue; 
        public float FloatValue => _floatValue;
        
        public void ApplyChange(float change)
        {
            _currentFloatValue += change;
        }

        private void OnEnable()
        {
            _currentFloatValue = _floatValue;
        }

    }

}