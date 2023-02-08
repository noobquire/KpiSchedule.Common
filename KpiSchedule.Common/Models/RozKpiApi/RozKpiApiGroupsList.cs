using System.Collections;

namespace KpiSchedule.Common.Models.RozKpiApi
{
    /// <summary>
    /// List of group names from roz.kpi.ua API
    /// </summary>
    public class RozKpiApiGroupsList : BaseRozKpiApiResponse
    {
        /// <summary>
        /// Prefix of groups in the list.
        /// </summary>
        public string GroupPrefix { get; set; }
    }
}
