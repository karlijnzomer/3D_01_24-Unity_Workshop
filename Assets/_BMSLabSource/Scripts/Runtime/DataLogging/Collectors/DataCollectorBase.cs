using MyBox;
using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.DataLogging.Collectors
{
    public abstract class DataCollectorBase : MonoBehaviour, ILoggable
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("The order in which this data source is registered and will appear in the data log. A lower number means the data column will appear first in the output log.")]
        private int _order = 1;
        public int Order => _order;
        [SerializeField, Tooltip("Specify the header that should be written on top of the file for this data source.")]
        protected bool _useCustomHeaderName = false;
        [ConditionalField(nameof(_useCustomHeaderName), false)]
        public string HeaderName = "MyCustomHeaderName";

        public abstract void Initialize();

        public abstract void OnStartLogging();

        public abstract string GetHeader();

        public abstract string LogData();

        public abstract void OnStopLogging();

    }
}
