using System.Runtime.Serialization;

namespace KpiSchedule.Common.Exceptions
{
    /// <summary>
    /// Exception indicating an issue with KpiSchedule clients used to call external APIs.
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleClientException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public KpiScheduleClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleClientException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected KpiScheduleClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
