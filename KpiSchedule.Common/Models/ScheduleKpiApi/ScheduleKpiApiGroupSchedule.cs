using System.Text.Json.Serialization;

namespace KpiSchedule.Common.Models.ScheduleKpiApi
{
    /// <summary>
    /// Model for group schedule from schedule.kpi.ua API
    /// </summary>
    public class ScheduleKpiApiGroupSchedule
    {
        /// <summary>
        /// Unique group identifier.
        /// </summary>
        [JsonPropertyName("groupCode")]
        public string GroupId { get; set; }

        /// <summary>
        /// First week of the schedule.
        /// </summary>
        public IList<ScheduleKpiApiDay> ScheduleFirstWeek { get; set; }

        /// <summary>
        /// Second week of the schedule.
        /// </summary>
        public IList<ScheduleKpiApiDay> ScheduleSecondWeek { get; set; }
    }
}
