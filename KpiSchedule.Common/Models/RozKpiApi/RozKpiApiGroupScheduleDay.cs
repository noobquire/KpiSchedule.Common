﻿namespace KpiSchedule.Common.Models.RozKpiApi
{
    public class RozKpiApiGroupScheduleDay
    {
        /// <summary>
        /// 1-based day number.
        /// </summary>
        public int DayNumber { get; set; }

        /// <summary>
        /// List of pairs this day.
        /// </summary>
        public IList<RozKpiApiGroupPair> Pairs { get; set; }
    }
}