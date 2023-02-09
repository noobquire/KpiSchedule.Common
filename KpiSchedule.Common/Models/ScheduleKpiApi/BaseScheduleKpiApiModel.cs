using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiSchedule.Common.Models.ScheduleKpiApi
{
    /// <summary>
    /// Base model for schedule.kpi.ua API resources.
    /// </summary>
    public class BaseScheduleKpiApiModel
    {
        /// <summary>
        /// Unique resource identifier.
        /// </summary>
        public string Id { get; set; }
    }
}
