namespace KpiSchedule.Common.Models.RozKpiApi
{
    public class RozKpiApiTeacherPair : BaseRozKpiApiPair
    {
        /// <summary>
        /// List of groups for this pair.
        /// </summary>
        public IList<string> GroupNames { get; set; }
    }
}
