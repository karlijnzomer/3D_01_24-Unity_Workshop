using MyBox;
using System.Collections;
using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.DataLogging.Collectors
{
    public class CustomDataCollector : DataCollectorBase, ILoggable
    {
        [Space(20)]
        [ReadOnly]
        public DataType LogType;

        private string _data;

        public enum DataType { String }

        public override void Initialize()
        {
            _data = "";
        }

        public override string GetHeader()
        {
            if (_useCustomHeaderName)
            {
                return HeaderName;
            }
            else
            {
                return "CustomDataCollector";
            }
        }

        public override void OnStartLogging()
        {
        }

        public override string LogData()
        {
            string data = _data;
            DataLogManager.Instance.AggregateData(GetHeader(), data);
            StartCoroutine(WaitOneFrameAndResetData());
            return null; // This will return the actual data when available, or the placeholder otherwise
        }

        public void CollectDataEntry(string data)
        {
            _data = data;
        }

        private IEnumerator WaitOneFrameAndResetData()
        {
            yield return 0;
            _data = "";
        }
 
        public override void OnStopLogging()
        {
        }
    }
}
