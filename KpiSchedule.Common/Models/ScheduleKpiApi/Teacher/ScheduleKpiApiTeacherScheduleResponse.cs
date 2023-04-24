using KpiSchedule.Common.Models.ScheduleKpiApi.Base;

namespace KpiSchedule.Common.Models.ScheduleKpiApi.Teacher
{
    /// <summary>
    /// Response from schedule.kpi.api Get Teacher Schedule API.
    /// </summary>
    public class ScheduleKpiApiTeacherScheduleResponse : BaseScheduleKpiApiResponse<ScheduleKpiApiTeacherSchedule>
    {
        /// <summary>
        /// Schedule data.
        /// </summary>
        public new ScheduleKpiApiTeacherSchedule Data { get; set; }
    }
}
