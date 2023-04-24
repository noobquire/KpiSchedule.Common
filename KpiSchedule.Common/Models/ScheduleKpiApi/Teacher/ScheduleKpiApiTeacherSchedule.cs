using System.Text.Json.Serialization;
using KpiSchedule.Common.Models.ScheduleKpiApi.Base;

namespace KpiSchedule.Common.Models.ScheduleKpiApi.Teacher
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
