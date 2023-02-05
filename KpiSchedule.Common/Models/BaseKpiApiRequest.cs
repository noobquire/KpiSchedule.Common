using System.Text.Json.Serialization;

namespace KpiSchedule.Common.Models
{
    /// <summary>
    /// Base request model for calling KPI API.
    /// </summary>
    [Serializable]
    internal class BaseKpiApiRequest
    {
        /// <summary>
        /// Prefix text query.
        /// </summary>
        [JsonPropertyName("prefixText")]
        public string PrefixText { get; init; }

        /// <summary>
        /// Initialize a new instance of the <see cref="BaseKpiApiRequest"/> class.
        /// </summary>
        /// <param name="prefixText">Prefix text query.</param>
        public BaseKpiApiRequest(string prefixText)
        {
            PrefixText = prefixText;
        }
    }
}
