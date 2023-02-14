using System.Text.Json.Serialization;

namespace KpiSchedule.Common.Models.RozKpiApi
{
    /// <summary>
    /// Base class for responses from roz.kpi.ua API
    /// </summary>
    public class BaseRozKpiApiResponse
    {
        /// <summary>
        /// Data array with requested resources.
        /// </summary>
        [JsonPropertyName("d")]
        public IList<string> Data { get; set; } = new List<string>();
    }
}
