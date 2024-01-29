using System;
using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.DataLogging.Collectors
{
    public class TimeDataCollector : DataCollectorBase, ILoggable
    {
        [Space(20)]
        public TimeLogType TimeType = TimeLogType.CurrentHoursMinutesSeconds;

        public enum TimeLogType { CurrentDate, CurrentHours, CurrentHoursMinutes, CurrentHoursMinutesSeconds, TimeSinceStartup }

        public override void Initialize()
        {
        }
        public override string GetHeader()
        {
            if (_useCustomHeaderName)
            {
                return HeaderName;
            }
            else
            {
                return TimeType switch
                {
                    TimeLogType.CurrentDate => "Current Date",
                    TimeLogType.CurrentHours => "Curent Time (HH)",
                    TimeLogType.CurrentHoursMinutes => "Current Time (HH:mm)",
                    TimeLogType.CurrentHoursMinutesSeconds => "Current Time (HH:mm:ss)",
                    TimeLogType.TimeSinceStartup => "Time since Startup (HH:mm:ss)",
                    _ => null,
                };
            }
        }

        public override void OnStartLogging()
        {
        }


        public override string LogData()
        {
            string data = GetTimeData();
            if (data != null)
            {
                DataLogManager.Instance.AggregateData(GetHeader(), data);
            }
            return null;
        }

        private string GetTimeData()
        {
            switch (TimeType)
            {
                case TimeLogType.CurrentDate:
                    return DateTime.Now.ToString(DateTime.Now.ToString("dd:MM:YYYY"));
                case TimeLogType.CurrentHours:
                    return DateTime.Now.ToString(DateTime.Now.ToString("HH"));
                case TimeLogType.CurrentHoursMinutes:
                    return DateTime.Now.ToString(DateTime.Now.ToString("HH:mm"));
                case TimeLogType.CurrentHoursMinutesSeconds:
                    return DateTime.Now.ToString(DateTime.Now.ToString("HH:mm:ss"));
                case TimeLogType.TimeSinceStartup:
                    TimeSpan timeSinceStartup = TimeSpan.FromSeconds(Time.time);
                    return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSinceStartup.Hours, timeSinceStartup.Minutes, timeSinceStartup.Seconds);
                default:
                    return null;
            }
        }

        public override void OnStopLogging()
        {
        }
    }
}

