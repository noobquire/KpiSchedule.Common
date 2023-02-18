namespace KpiSchedule.Common.Models.RozKpiApi
{
    /// <summary>
    /// Academic group schedule parsed from roz.kpi.ua.
    /// </summary>
    public class RozKpiApiGroupSchedule : BaseRozKpiApiSchedule<RozKpiApiGroupScheduleDay>
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string GroupName { get; set; }
    }
}
