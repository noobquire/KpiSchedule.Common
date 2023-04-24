using KpiSchedule.Common.Models.RozKpiApi.Teacher;

namespace KpiSchedule.Common.Models.RozKpiApi.Group
{
    public class RozKpiApiGroupPairData
    {
        public IEnumerable<string> SubjectNames { get; set; }
        public IEnumerable<string> FullSubjectNames { get; set; }
        public RozKpiApiTeacher[][] Teachers { get; set; }
        public IEnumerable<RozKpiApiPairInfo> PairInfos { get; set; }

        public PairIdentifier Identifier { get; set; }
    }
}
