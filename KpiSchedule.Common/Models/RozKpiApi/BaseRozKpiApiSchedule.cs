namespace KpiSchedule.Common.Models.RozKpiApi
{
    public abstract class BaseRozKpiApiSchedule<TScheduleDay>
    {
        /// <summary>
        /// Schedule identifier on roz.kpi.ua.
        /// </summary>
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// First week of the schedule.
        /// </summary>
        public IList<TScheduleDay> FirstWeek { get; set; }

        /// <summary>
        /// Second week of the schedule.
        /// </summary>
        public IList<TScheduleDay> SecondWeek { get; set; }
    }
}
