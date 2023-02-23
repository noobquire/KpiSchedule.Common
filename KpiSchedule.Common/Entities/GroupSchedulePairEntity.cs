namespace KpiSchedule.Common.Entities
{
    /// <summary>
    /// Group schedule pair DB entity.
    /// </summary>
    public class GroupSchedulePairEntity : BaseSchedulePairEntity
    {
        /// <summary>
        /// Teacher(s) which conduct this pair.
        /// </summary>
        public List<TeacherEntity> Teachers { get; set; }
    }
}