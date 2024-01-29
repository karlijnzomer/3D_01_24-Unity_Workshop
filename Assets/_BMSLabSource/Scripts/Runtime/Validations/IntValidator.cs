using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.Validations
{
    public class IntValidator : MonoBehaviour
    {
        [SerializeField]
        private string _description;

        public OperatorType Operator;

        [SerializeField]
        private int _currentCount = 0;

        [SerializeField]
        private int _referenceValue;
        public enum OperatorType { GREATER_OR_EQUAL };

        public UnityEvent ValidatationValid, ValidationInvalid;

        public void Validate()
        {
            switch (Operator)
            {
                case OperatorType.GREATER_OR_EQUAL:
                    if (_currentCount >= _referenceValue)
                    {
                        ValidatationValid.Invoke();
                    }
                    else
                    {
                        ValidationInvalid.Invoke();
                    }
                    break;
            }
        }

        public void IncrementCurrentCount()
        {
            _currentCount++;

            Validate();
        }

        public void SetCurrentCount(int targetCount)
        {
            _currentCount = targetCount;

            Validate();
        }
    }
}
