using System.Text.Json.Serialization;

namespace KpiSchedule.Common.Models.ScheduleKpiApi
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
