using System.Runtime.Serialization;

namespace KpiSchedule.Common.Exceptions
{
    /// <summary>
    /// Exception indicating that a call to get group schedule id resulted in a "group not found" page.
    /// </summary>
    [Serializable]
    public class KpiScheduleClientGroupNotFoundException : KpiScheduleClientException
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleClientGroupNotFoundException"/> class.
        /// </summary>
        public KpiScheduleClientGroupNotFoundException() : base()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleClientGroupNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public KpiScheduleClientGroupNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleClientGroupNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public KpiScheduleClientGroupNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleClientGroupNotFoundException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected KpiScheduleClientGroupNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
