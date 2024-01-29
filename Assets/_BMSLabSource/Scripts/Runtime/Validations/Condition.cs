using UnityEngine;
using UnityEngine.Events;

namespace _BMSLabSource.Scripts.Runtime.Validations
{
    public class Condition : MonoBehaviour
    {
        [SerializeField]
        private string _description;

        [SerializeField]
        private bool _condition = false;

        public UnityEvent ConditionMet, ConditionNotMet;

        /// <summary>
        /// Sets the condition to true.
        /// </summary>
        public void SetTrue()
        {
            _condition = true;
        }

        /// <summary>
        /// Sets the condition to false.
        /// </summary>
        public void SetFalse()
        {
            _condition = false;
        }

        /// <summary>
        /// Validates the condition.
        /// </summary>
        public void Validate()
        {
            if (_condition == true)
            {
                ConditionMet?.Invoke();
            }
            else
            {
                ConditionNotMet?.Invoke();
            }
        }
    }
}
