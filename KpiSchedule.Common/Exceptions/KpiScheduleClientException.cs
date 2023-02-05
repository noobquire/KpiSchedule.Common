namespace KpiSchedule.Common.Exceptions
{
    /// <summary>
    /// An exception indicating an issue with KpiSchedule clients used to call external APIs.
    /// </summary>
    public class KpiScheduleClientException : ApplicationException
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleClientException"/> class.
        /// </summary>
        public KpiScheduleClientException() : base()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleClientException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public KpiScheduleClientException(string message) : base(message)
        {
        }
    }
}
