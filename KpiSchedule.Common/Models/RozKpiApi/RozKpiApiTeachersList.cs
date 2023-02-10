namespace KpiSchedule.Common.Models.RozKpiApi
{
    /// <summary>
    /// List of teacher names from roz.kpi.ua API
    /// </summary>
    public class RozKpiApiTeachersList : BaseRozKpiApiResponse
    {
        /// <summary>
        /// Prefix of teacher names in the list.
        /// </summary>
        public string TeacherNamePrefix { get; set; }
    }
}
