using System.Text.Json.Serialization;

namespace KpiSchedule.Common.Models
{
    /// <summary>
    /// Base class for responses from roz.kpi.ua API
    /// </summary>
    public class BaseKpiApiResponse
    {
        /// <summary>
        /// Data array with requested resources.
        /// </summary>
        [JsonPropertyName("d")]
        public IList<string>? Data { get; set; }
    }
}
