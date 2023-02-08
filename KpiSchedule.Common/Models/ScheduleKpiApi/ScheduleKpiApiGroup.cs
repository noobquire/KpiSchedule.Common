using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KpiSchedule.Common.Models.ScheduleKpiApi
{
    /// <summary>
    /// Model for academic groups from schedule.kpi.ua API.
    /// </summary>
    public class ScheduleKpiApiGroup : BaseScheduleKpiApiModel
    {
        /// <summary>
        /// Group name/cipher.
        /// </summary>
        [JsonPropertyName("name")]
        public string GroupName { get; set; }

        /// <summary>
        /// Group faculty.
        /// </summary>
        public string Faculty { get; set; }
    }
}
