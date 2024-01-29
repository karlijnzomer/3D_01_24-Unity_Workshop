using System;
using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.ScriptableObjects.Variables
{
    [CreateAssetMenu(fileName = "NewFloatVariable", menuName = "Scriptable Objects/Variables/Float Variable")]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField, Tooltip("Optional field to provide a description for this variable.")]
        private string _description;

        [SerializeField, Tooltip("The value that is stored in this scriptable object.")]
        private float _value;

        [Serializable]
        public class ValueChangedEvent : UnityEvent<float> { }

        [Tooltip("A UnityEvent that gets invoked when the current value changes to a different value.")]
        public ValueChangedEvent ValueChanged = new();


        public float Value
        {
            get { return _value; }
            set
            {
                if (value == _value)
                    return;

                _value = value;
                OnValueChanged(value);
            }
        }

        private void OnValueChanged(float newValue)
        {
            ValueChanged?.Invoke(newValue);
        }
    }
}