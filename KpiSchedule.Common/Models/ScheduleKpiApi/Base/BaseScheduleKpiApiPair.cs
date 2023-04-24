namespace KpiSchedule.Common.Models.ScheduleKpiApi.Base
{
    /// <summary>
    /// Base class for pairs in schedule.kpi.ua API
    /// </summary>
    public class BaseScheduleKpiApiPair
    {
        /// <summary>
        /// Pair type description: lecture/lab/prac + on-line?
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Pair start time, in HH.mm format.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Subject name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Room number where pair takes place.
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// Pair tag indicating its type. Values: lec, lab, prac
        /// </summary>
        public string Tag { get; set; }
    }
}
