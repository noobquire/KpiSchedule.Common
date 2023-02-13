namespace KpiSchedule.Common.Models.RozKpiApi
{
    public class RozKpiApiGroupPair : BaseRozKpiApiPair
    {
        /// <summary>
        /// List of teachers for this pair.
        /// </summary>
        public IList<RozKpiApiTeacher> Teachers { get; set; }
    }
}