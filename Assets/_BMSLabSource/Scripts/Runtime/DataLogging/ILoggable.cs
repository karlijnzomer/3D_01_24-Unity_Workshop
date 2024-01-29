
namespace _BMSLabSource.Scripts.Runtime.DataLogging
{
    public interface ILoggable
    {
        /// <summary>
        /// The order in which the data source is registered and will appear in the data log.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Initialize the loggable data source, e.g., setup references or configurations.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Implement this method to set a header name for the data log specific to the data source.
        /// </summary>
        /// <returns></returns>
        string GetHeader();

        /// <summary>
        /// Called when logging is started.
        /// </summary>
        void OnStartLogging();

        /// <summary>
        /// Implement this method to collect data specific to the data source.
        /// </summary>
        /// <returns></returns>
        string LogData();

        /// <summary>
        /// Called when logging is stopped.
        /// </summary>
        void OnStopLogging();

    }
}
