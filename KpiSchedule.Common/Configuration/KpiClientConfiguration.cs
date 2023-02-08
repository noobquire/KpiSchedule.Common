namespace KpiSchedule.Common.Configuration
{
    /// <summary>
    /// Configuration model for KPI API clients.
    /// </summary>
    public class KpiClientConfiguration
    {
        /// <summary>
        /// Base address URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Request timeout in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; }
    }
}
