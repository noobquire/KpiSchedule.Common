namespace KpiSchedule.Common.Models.ScheduleKpiApi.Time
{
    /// <summary>
    /// Model for schedule time info from schedule.kpi.ua API.
    /// </summary>
    public class ScheduleKpiApiTimeInfo
    {
        /// <summary>
        /// 1-based number of the current week.
        /// </summary>
        public int CurrentWeek { get; set; }

        /// <summary>
        /// 1-based number of the current day.
        /// </summary>
        public int CurrentDay { get; set; }

        /// <summary>
        /// 1-based number of the current lesson.
        /// 0 if there is no current lesson?
        /// </summary>
        public int CurrentLesson { get; set; }
    }
}
