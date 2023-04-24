using System.Text.Json.Serialization;
using KpiSchedule.Common.Models.ScheduleKpiApi.Base;

namespace KpiSchedule.Common.Models.ScheduleKpiApi.Teacher
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
