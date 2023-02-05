using System.Collections;

namespace KpiSchedule.Common.Models
{
    /// <summary>
    /// List of group names from roz.kpi.ua API
    /// </summary>
    public class KpiApiGroupsList : BaseKpiApiResponse, IEnumerable<string>
    {
        /// <summary>
        /// Prefix of groups in the list.
        /// </summary>
        public string? GroupPrefix { get; set; }

        public IEnumerator<string> GetEnumerator() => Data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
