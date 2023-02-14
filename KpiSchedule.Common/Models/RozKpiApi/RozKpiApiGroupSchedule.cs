namespace KpiSchedule.Common.Models.RozKpiApi
{
    /// <summary>
    /// Academic group schedule parsed from roz.kpi.ua.
    /// </summary>
    public class RozKpiApiGroupSchedule
    {
        /// <summary>
        /// Schedule identifier on roz.kpi.ua.
        /// </summary>
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// Group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// First week of the schedule.
        /// </summary>
        public IList<RozKpiApiGroupScheduleDay> FirstWeek { get; set; }

        /// <summary>
        /// Second week of the schedule.
        /// </summary>
        public IList<RozKpiApiGroupScheduleDay> SecondWeek { get; set; }
    }
}
