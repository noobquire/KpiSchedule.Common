﻿namespace KpiSchedule.Common.Models.ScheduleKpiApi.Responses
{
    /// <summary>
    /// Response from schedule.kpi.api Get Group Schedule API.
    /// </summary>
    public class ScheduleKpiApiTimeResponse : BaseScheduleKpiApiResponse<ScheduleKpiApiTimeInfo>
    {
        /// <summary>
        /// Time info data.
        /// </summary>
        public new ScheduleKpiApiTimeInfo Data { get; set; }
    }
}
