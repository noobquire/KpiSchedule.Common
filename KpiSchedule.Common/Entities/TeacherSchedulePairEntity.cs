namespace KpiSchedule.Common.Entities
{
    /// <summary>
    /// Teacher pair DB entity.
    /// </summary>
    public class TeacherSchedulePairEntity : BaseSchedulePairEntity
    {
        /// <summary>
        /// Groups for which this pair is conducted.
        /// </summary>
        public List<GroupEntity> Groups { get; set; }
    }
}
