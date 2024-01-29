using MyBox;
using System.Reflection;
using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.DataLogging.Collectors
{
    // TODO: Assert that target variable exists/can be found & print current value
    // NOTE: Data entries that are logged within the same frame using this component will be appended within the same cell without any seperators.

    public class ComponentDataCollector : DataCollectorBase, ILoggable
    {
        [Space(20)]
        [SerializeField, Tooltip("The component from which you would like to source data from.")]
        private Component _targetComponent;
        [SerializeField, Tooltip("The variable name of the component which you would like to sample. Note: The variable name needs to exactly match the variable name within the script.")]
        private string _targetVariable;
        [ReadOnly]
        public DataType LogType;

        public enum DataType { VARIABLE }

        public override void Initialize()
        {
            if (_targetComponent == null)
            {
                Debug.LogError("Missing target component reference. Cannot retrieve data.", gameObject);
            }
        }

        public override string GetHeader()
        {
            if (_useCustomHeaderName)
            {
                return HeaderName;
            }
            else
            {
                return _targetComponent.name + " " + _targetVariable.ToString();
            }
        }

        public override void OnStartLogging()
        {
        }

        public override string LogData()
        {
            if (_targetComponent == null)
            {
                Debug.LogError("Missing target component reference. Cannot log data.", gameObject);
                return null;
            }

            FieldInfo field = _targetComponent.GetType().GetField(_targetVariable, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                string data = field.GetValue(_targetComponent).ToString();
                DataLogManager.Instance.AggregateData(GetHeader(), data);
                return null;
            }
            else
            {
                Debug.LogError("Cannot find or access variable: " + _targetVariable, this);
                return null;
            }
        }

        public override void OnStopLogging()
        {
        }
    }
}
