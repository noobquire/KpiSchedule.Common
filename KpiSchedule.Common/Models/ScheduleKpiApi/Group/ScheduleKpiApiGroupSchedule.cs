using System.Text.Json.Serialization;
using KpiSchedule.Common.Models.ScheduleKpiApi.Base;

namespace KpiSchedule.Common.Models.ScheduleKpiApi.Group
{
    /// <summary>
    /// Model for group schedule from schedule.kpi.ua API
    /// </summary>
    public class ScheduleKpiApiGroupSchedule : BaseScheduleKpiApiSchedule
    {
        /// <summary>
        /// Unique group identifier.
        /// </summary>
        [JsonPropertyName("groupCode")]
        public string GroupId { get; set; }
    }
}
