using System.Text.Json.Serialization;

namespace KpiSchedule.Common.Models
{
    /// <summary>
    /// Base request model for calling roz.kpi.ua API.
    /// </summary>
    [Serializable]
    internal class BaseRozKpiApiRequest
    {
        /// <summary>
        /// Prefix text query.
        /// </summary>
        [JsonPropertyName("prefixText")]
        public string PrefixText { get; init; }

        /// <summary>
        /// Initialize a new instance of the <see cref="BaseRozKpiApiRequest"/> class.
        /// </summary>
        /// <param name="prefixText">Prefix text query.</param>
        public BaseRozKpiApiRequest(string prefixText)
        {
            PrefixText = prefixText;
        }
    }
}
