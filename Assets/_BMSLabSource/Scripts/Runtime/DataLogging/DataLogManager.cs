using MyBox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace _BMSLabSource.Scripts.Runtime.DataLogging
{
    public class DataLogManager : MonoBehaviour
    {
        // TODO: ColorPerception_participantID_conditionID_(time)
        // TODO: Make sure that data collectors register themselves on runtime (use OnEnable)
        // TODO: Implement sample rate
        // TODO: Performance test in actual production using hundreds of objects

        [Header("Logging Configuration")]
        [SerializeField, Tooltip("Enable data logging.")]
        private bool _enableLogging = true;
        [SerializeField, Tooltip("Write log messages to the console.")]
        private bool _debug = false;
        public static DataLogManager Instance;
        [ReadOnly, Tooltip("The interval at which to log data. At this interval a new row of data is written to the file. At the moment only logging data every frame is supported.")]
        public SampleRateMode SampleRate = SampleRateMode.Frame;
        public enum SampleRateMode { Frame }
        [Tooltip("When the data logging should start. Default is on Start of the Application."), ReadOnly]
        public LogStartMode StartMode = LogStartMode.OnStart;
        public enum LogStartMode { OnStart, AfterSeconds, Manual }
        [ReadOnly, SerializeField, Min(0), ConditionalField(nameof(StartMode), false, LogStartMode.AfterSeconds), Tooltip("The amount of seconds to wait before starting to log data. This is only used when the Start Mode is set to After Seconds.")]
        private float _startTime = 1f;

        [Separator]
        [Header("File Settings")]
        [SerializeField, Tooltip("The name for the output data log file.")]
        private string _fileName = "YourFileNameHere";
        [SerializeField, Tooltip("Automatically append the current time to the file name.")]
        private bool _appendTimestamp = true;
        [Tooltip("Choose the file type/extension for the data log file.")]
        public FileTypeMode FileType = FileTypeMode.CSV;
        public enum FileTypeMode { CSV, TXT }
        [Tooltip("The delimiter is the default seperator for Excel to seperate data entries into multiple columns.")]
        public DelimiterMode Delimiter = DelimiterMode.Semicolon;
        public enum DelimiterMode { Semicolon, Comma }

        [Header("Folder Settings")]
        [SerializeField, Tooltip("The folder within this project where the data log file(s) will be stored.")]
        private string _folderName = "Logs";

        private List<ILoggable> _loggableDataSources = new();
        private Dictionary<string, string> _aggregatedData = new Dictionary<string, string>();
        private StreamWriter _logWriter;
        private string _fileExtension = ".csv";
        private string _filePath;
        private string _delimiter;
        private bool _headerWritten = false;
        private bool _loggingActive = false;
        private bool _isCurrentlyLogging = false;

        private void Awake()
        {
            if (_enableLogging == false || enabled == false)
                return;

            SetInstance();
            HandleFileSettings();
            InitializeFile();
        }

        private void Start()
        {
            if (_enableLogging == false || enabled == false)
                return;

            RetrieveLoggableDataSources();
            CollectHeaders();
            WriteHeaders();

            if (StartMode == LogStartMode.OnStart)
            {
                StartLogging();
            }
            else if (StartMode == LogStartMode.AfterSeconds)
            {
                Invoke(nameof(StartLogging), _startTime);
            }
            else if (StartMode == LogStartMode.Manual)
            {
                // Do nothing, since the StartLogging method would be called manually
            }
        }

        /// <summary>
        /// Starts logging data. Only call this method if the Start Mode is set to Manual.
        /// </summary>
        public void StartLogging()
        {
            _loggingActive = true;

            if (_debug)
                Debug.Log("Data Log Manager: " + "Started logging data.", gameObject);
        }

        private void Update()
        {
            if (_enableLogging == false || enabled == false)
                return;

            if (_headerWritten == false)
                return;

            if (_loggingActive == false)
                return;

            LogData();
        }

        private void OnApplicationQuit()
        {
            StopLogging();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        private void SetInstance()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void HandleFileSettings()
        {
            switch (Delimiter)
            {
                case DelimiterMode.Semicolon:
                    _delimiter = ";";
                    break;
                case DelimiterMode.Comma:
                    _delimiter = ",";
                    break;
                default:
                    goto case DelimiterMode.Semicolon;
            }

            switch (FileType)
            {
                case FileTypeMode.CSV:
                    _fileExtension = ".csv";
                    break;
                case FileTypeMode.TXT:
                    _fileExtension = ".txt";
                    break;
                default:
                    goto case FileTypeMode.CSV;
            }
        }

        private void InitializeFile()
        {
            string customFolder = Path.Combine(Application.dataPath, _folderName);

            if (!Directory.Exists(customFolder))
            {
                Directory.CreateDirectory(customFolder);
            }

            if (_fileName == "")
            {
                _fileName = "DataLog";
            }

            if (_appendTimestamp)
            {
                _fileName += " " + DateTime.Now.ToString(DateTime.Now.ToString("HH-mm-ss"));
            }

            if (!_fileName.EndsWith(_fileExtension))
            {
                _fileName += _fileExtension;
            }

            _filePath = Path.Combine(customFolder, _fileName);
            _logWriter = new StreamWriter(_filePath);

            if (_debug)
                Debug.Log("Data Log Manager: " + "Initialized file: " + _filePath, gameObject);
        }

        private void RetrieveLoggableDataSources()
        {
            ILoggable[] dataSources = FindObjectsOfType<MonoBehaviour>()
                           .OfType<ILoggable>()
                           .OrderBy(loggable => loggable.Order)
                           .ToArray();

            foreach (var dataSource in dataSources)
            {
                if (_debug)
                    Debug.Log("Data Log Manager: " + "Registered a data source for logging: " + dataSource, gameObject);

                _loggableDataSources.Add(dataSource);
                dataSource.Initialize();
            }

            if (_debug)
                Debug.Log("Registered " + _loggableDataSources.Count + " data sources.", gameObject);
        }

        private void CollectHeaders()
        {
            foreach (var dataSource in _loggableDataSources)
            {
                string header = dataSource.GetHeader();
                if (!_aggregatedData.ContainsKey(header))
                {
                    _aggregatedData.Add(header, "");
                }
            }
        }

        private void WriteHeaders()
        {
            foreach (var header in _aggregatedData.Keys)
            {
                _logWriter.Write(header + _delimiter);
            }
            _logWriter.WriteLine();
            _headerWritten = true;

            if (_debug)
                Debug.Log($"Data Log Manager: Wrote {_aggregatedData.Keys.Count} headers to file.", gameObject);
        }

        /// <summary>
        /// Data Collector classes call this method to aggregate their data. Do not call manually.
        /// </summary>
        /// <param name="header">The header that is sent by the data collector.</param>
        /// <param name="data">The data that is sent by the data collector.</param>
        public void AggregateData(string header, string data)
        {
            if (_aggregatedData.ContainsKey(header))
            {
                if (!string.IsNullOrEmpty(_aggregatedData[header]))
                {
                    // If there's existing data, append the new data with a delimiter.
                    //_aggregatedData[header] += " | " + data;
                    _aggregatedData[header] += data;
                }
                else
                {
                    _aggregatedData[header] = data;
                }
            }
            else
            {
                _aggregatedData.Add(header, data);
            }
        }

        private void LogData()
        {
            _isCurrentlyLogging = true;
            _aggregatedData.Clear();

            foreach (ILoggable loggable in _loggableDataSources)
            {
                string header = loggable.GetHeader();
                string data = loggable.LogData();

                if (!_aggregatedData.ContainsKey(header))
                {
                    _aggregatedData[header] += data;
                    Debug.Log("Why is this if block never being called?");
                }
                else
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        _aggregatedData[header] += data;
                    }
                }
            }

            WriteAggregatedData();
        }

        private void WriteAggregatedData()
        {
            foreach (var data in _aggregatedData.Values)
            {
                _logWriter.Write(data + _delimiter);
            }
            _logWriter.WriteLine();
        }

        private void StopLogging()
        {
            if (_isCurrentlyLogging)
            {
                foreach (var dataSource in _loggableDataSources)
                {
                    dataSource.OnStopLogging();
                }

                _logWriter.Close();

                _isCurrentlyLogging = false;
            }
        }
    }
}