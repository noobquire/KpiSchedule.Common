using System.Runtime.Serialization;

namespace KpiSchedule.Common.Exceptions
{
    /// <summary>
    /// Exception indicating an issue with KpiSchedule parsers used to parse data from HTML documents.
    /// </summary>
    [Serializable]
    public class KpiScheduleParserException : ApplicationException
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleParserException"/> class.
        /// </summary>
        public KpiScheduleParserException() : base()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleParserException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public KpiScheduleParserException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleParserException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public KpiScheduleParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiScheduleClientException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected KpiScheduleParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
