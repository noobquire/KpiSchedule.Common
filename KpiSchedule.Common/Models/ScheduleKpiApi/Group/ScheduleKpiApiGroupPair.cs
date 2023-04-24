using System.Text.Json.Serialization;
using KpiSchedule.Common.Models.ScheduleKpiApi.Base;

namespace KpiSchedule.Common.Models.ScheduleKpiApi.Group
{
    /// <summary>
    /// Schedule pair model in response from schedule.kpi.ua API.
    /// </summary>
    public class ScheduleKpiApiGroupPair : BaseScheduleKpiApiPair
    {
        /// <summary>
        /// Teacher name.
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// Unique identifier of the teacher.
        /// </summary>
        [JsonPropertyName("lecturerId")]
        public string TeacherId { get; set; }
    }
}
