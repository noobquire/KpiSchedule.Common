using System.Text.Json.Serialization;

namespace KpiSchedule.Common.Models.ScheduleKpiApi
{
    /// <summary>
    /// Teacher schedule pair model from schedule.kpi.ua API.
    /// </summary>
    internal class ScheduleKpiApiTeacherPair : BaseScheduleKpiApiPair
    {
        /// <summary>
        /// Group name/cipher.
        /// </summary>
        [JsonPropertyName("group")]
        public string GroupName { get; set; }
    }
}
