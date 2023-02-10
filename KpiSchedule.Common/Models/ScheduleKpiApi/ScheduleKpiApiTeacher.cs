namespace KpiSchedule.Common.Models.ScheduleKpiApi
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
