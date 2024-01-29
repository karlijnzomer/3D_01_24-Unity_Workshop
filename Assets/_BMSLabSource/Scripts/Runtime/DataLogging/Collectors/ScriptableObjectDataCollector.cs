using System.Reflection;
using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.DataLogging.Collectors
{
    public class ScriptableObjectDataCollector : DataCollectorBase, ILoggable
    {
        [Space(20)]
        [SerializeField]
        private ScriptableObject _targetScriptableObject;
        [SerializeField, Tooltip("The variable name of the ScriptableObject which you would like to sample. Note: The variable name needs to exactly match the variable name within the script.")]
        private string _targetVariable = "_value";

        public override void Initialize()
        {
            if (_targetScriptableObject == null)
            {
                Debug.LogError("Missing target ScriptableObject reference. Cannot retrieve data.", gameObject);
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
                return _targetScriptableObject.name + " ";
            }
        }

        public override void OnStartLogging()
        {
        }

        public override string LogData()
        {
            string data;

            FieldInfo field = _targetScriptableObject.GetType().GetField(_targetVariable, BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                data = field.GetValue(_targetScriptableObject).ToString();
                DataLogManager.Instance.AggregateData(GetHeader(), data);
                return null;
            }
            else
            {
                Debug.LogError("Can't find or access variable.", gameObject);
                return null;
            }
        }

        public override void OnStopLogging()
        {
        }
    }
}
