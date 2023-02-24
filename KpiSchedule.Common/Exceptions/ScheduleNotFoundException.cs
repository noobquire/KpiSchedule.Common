using System.Runtime.Serialization;

namespace KpiSchedule.Common.Exceptions
{
    /// <summary>
    /// Exception indicating that requested schedule was not found.
    /// </summary>
    [Serializable]
    public class ScheduleNotFoundException : ApplicationException
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="ScheduleNotFoundException"/> class.
        /// </summary>
        public ScheduleNotFoundException() : this("Requested schedule was not found.")
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ScheduleNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ScheduleNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ScheduleNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ScheduleNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ScheduleNotFoundException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected ScheduleNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
