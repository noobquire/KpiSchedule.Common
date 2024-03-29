﻿using KpiSchedule.Common.Models.ScheduleKpiApi.Group;

namespace KpiSchedule.Common.Models.ScheduleKpiApi.Base
{
    /// <summary>
    /// Base class for schedules in schedule.kpi.ua API
    /// </summary>
    public class BaseScheduleKpiApiSchedule
    {
        /// <summary>
        /// First week of the schedule.
        /// </summary>
        public IList<ScheduleKpiApiDay> ScheduleFirstWeek { get; set; }

        /// <summary>
        /// Second week of the schedule.
        /// </summary>
        public IList<ScheduleKpiApiDay> ScheduleSecondWeek { get; set; }
    }
}
