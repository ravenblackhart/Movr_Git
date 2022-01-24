using UnityEngine;

namespace _Scripts.ScriptableVariables
{
    [CreateAssetMenu(fileName = "new IntVariable", menuName = "ScriptableVariables/IntVariable")]
    public class IntVariable : ScriptableObject
    {
        [SerializeField] private int _intValue;
        private int _currentIntValue; 
        public int IntValue => _intValue;

        public void ApplyChange(int change)
        {
            _currentIntValue += change;
        }

        private void OnEnable()
        {
            _currentIntValue = _intValue;
        }
    }
}