using KpiSchedule.Common.Models.RozKpiApi.Base;
using KpiSchedule.Common.Models.RozKpiApi.Teacher;

namespace KpiSchedule.Common.Models.RozKpiApi.Group
{
    public class RozKpiApiGroupPair : BaseRozKpiApiPair
    {
        /// <summary>
        /// List of teachers for this pair.
        /// </summary>
        public IList<RozKpiApiTeacher> Teachers { get; set; }
    }
}