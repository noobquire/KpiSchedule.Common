namespace KpiSchedule.Common.Entities.RozKpi
{
    public class TeacherPairEntity : BasePairEntity
    {
        /// <summary>
        /// List of groups for this pair.
        /// </summary>
        public List<string> groupNames { get; set; }
    }
}
