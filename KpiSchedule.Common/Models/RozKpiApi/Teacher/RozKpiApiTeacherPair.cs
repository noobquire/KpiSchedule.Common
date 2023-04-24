using KpiSchedule.Common.Models.RozKpiApi.Base;

namespace KpiSchedule.Common.Models.RozKpiApi.Teacher
{
    public class RozKpiApiTeacherPair : BaseRozKpiApiPair
    {
        /// <summary>
        /// List of groups for this pair.
        /// </summary>
        public IList<string> GroupNames { get; set; }
    }
}
