using KpiSchedule.Common.Models.ScheduleKpiApi.Base;

namespace KpiSchedule.Common.Models.ScheduleKpiApi.Teacher
{
    /// <summary>
    /// Model for teachers from schedule.kpi.ua API.
    /// </summary>
    public class ScheduleKpiApiTeacher : BaseScheduleKpiApiModel
    {
        /// <summary>
        /// Lecturer name.
        /// </summary>
        public string Name { get; set; }
    }
}
