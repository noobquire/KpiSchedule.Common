using System.Runtime.Serialization;

namespace KpiSchedule.Common.Exceptions
{
    /// <summary>
    /// An exception indicating an issue with KpiSchedule clients used to call external APIs.
    /// </summary>
    [Serializable]
    public class KpiApiClientException : ApplicationException
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="KpiApiClientException"/> class.
        /// </summary>
        public KpiApiClientException() : base()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiApiClientException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public KpiApiClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiApiClientException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public KpiApiClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiApiClientException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected KpiApiClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
