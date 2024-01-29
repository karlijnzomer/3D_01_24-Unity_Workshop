using System;
using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.ScriptableObjects.Variables
{
    [CreateAssetMenu(fileName = "NewIntVariable", menuName = "Scriptable Objects/Variables/Int Variable")]
    public class IntVariable : ScriptableObject
    {
        [SerializeField, Tooltip("Optional field to provide a description for this variable.")]
        private string _description;

        [SerializeField, Tooltip("The value that is stored in this scriptable object.")]
        private int _value;

        [SerializeField, Min(0), Tooltip("If the value changes by this amount, the ValueChanged Event won't be invoked.")]
        private int _changeThreshold = 1;

        [Serializable]
        public class ValueChangedEvent : UnityEvent<int> { }

        [Tooltip("A UnityEvent that gets invoked when the current value changes to a different value.")]
        public ValueChangedEvent ValueChanged = new();


        public int Value
        {
            get { return _value; }
            set
            {
                if (value == _value)
                    return;

                if (Mathf.Abs(value - _value) > _changeThreshold)
                {
                    _value = value;
                    OnValueChanged(value);
                }

                _value = value;
            }
        }

        private void OnValueChanged(int newValue)
        {
            ValueChanged?.Invoke(newValue);
        }
    }
}