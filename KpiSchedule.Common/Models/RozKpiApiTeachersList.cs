using System.Collections;

namespace KpiSchedule.Common.Models
{
    /// <summary>
    /// List of teacher names from roz.kpi.ua API
    /// </summary>
    public class RozKpiApiTeachersList : BaseRozKpiApiResponse, IEnumerable<string>
    {
        /// <summary>
        /// Prefix of teacher names in the list.
        /// </summary>
        public string TeacherNamePrefix { get; set; }

        /// <inheritdoc/>
        public IEnumerator<string> GetEnumerator() => Data.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
