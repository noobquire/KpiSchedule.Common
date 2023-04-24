using KpiSchedule.Common.Models.ScheduleKpiApi.Base;

namespace KpiSchedule.Common.Models.ScheduleKpiApi.Group
{
    /// <summary>
    /// Response from schedule.kpi.api Get Group Schedule API.
    /// </summary>
    public class ScheduleKpiApiGroupScheduleResponse : BaseScheduleKpiApiResponse<ScheduleKpiApiGroupSchedule>
    {
        /// <summary>
        /// Schedule data.
        /// </summary>
        public new ScheduleKpiApiGroupSchedule Data { get; set; }
    }
}
