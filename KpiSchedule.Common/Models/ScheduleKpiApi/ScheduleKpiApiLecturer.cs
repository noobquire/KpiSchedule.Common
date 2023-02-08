namespace KpiSchedule.Common.Models.ScheduleKpiApi
{
    /// <summary>
    /// Model for lecturers/teachers from schedule.kpi.ua API.
    /// </summary>
    public class ScheduleKpiApiLecturer : BaseScheduleKpiApiModel
    {
        /// <summary>
        /// Lecturer name.
        /// </summary>
        public string Name { get; set; }
    }
}
