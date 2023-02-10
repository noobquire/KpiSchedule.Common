using System.Text.Json.Serialization;

namespace KpiSchedule.Common.Models.ScheduleKpiApi
{
    /// <summary>
    /// Model for teacher schedule from schedule.kpi.ua API
    /// </summary>
    public class ScheduleKpiApiTeacherSchedule : BaseScheduleKpiApiSchedule
    {
        [JsonPropertyName("lecturerName")]
        public string TeacherId { get; set; }
    }
}
