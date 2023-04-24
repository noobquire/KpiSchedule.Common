using System.Text.Json.Serialization;
using KpiSchedule.Common.Models.ScheduleKpiApi.Base;

namespace KpiSchedule.Common.Models.ScheduleKpiApi.Group
{
    /// <summary>
    /// Model for academic groups from schedule.kpi.ua API.
    /// </summary>
    public class ScheduleKpiApiGroup : BaseScheduleKpiApiModel
    {
        /// <summary>
        /// Group name/cipher.
        /// </summary>
        [JsonPropertyName("name")]
        public string GroupName { get; set; }

        /// <summary>
        /// Group faculty.
        /// </summary>
        public string Faculty { get; set; }
    }
}
