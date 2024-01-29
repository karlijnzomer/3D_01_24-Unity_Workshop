using MyBox;
using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.DataLogging.Collectors
{
    public class TransformDataCollector : DataCollectorBase, ILoggable
    {
        // TODO: Implement Different types of rotation (Quaternion, eulerAngles, localPos, worldPos)

        [Space(20)]
        [SerializeField]
        private Transform _targetTransform;
        public LogType DataType;

        public enum LogType { Position, Rotation, Scale }

        public override void Initialize()
        {
            if (_targetTransform == null)
            {
                _targetTransform = transform;
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
                return _targetTransform.name + " " + DataType;
            }
        }

        public override void OnStartLogging()
        {
        }

        public override string LogData()
        {
            string data = GetTransformData();
            if (data != null)
            {
                DataLogManager.Instance.AggregateData(GetHeader(), data);
            }
            return null; // Or return an empty string
        }

        private string GetTransformData()
        {
            return DataType switch
            {
                LogType.Position => _targetTransform.position.ToString(),
                LogType.Rotation => _targetTransform.rotation.ToString(),
                LogType.Scale => _targetTransform.localScale.ToString(),
                _ => null,
            };
        }

        public override void OnStopLogging()
        {
        }

    }
}
