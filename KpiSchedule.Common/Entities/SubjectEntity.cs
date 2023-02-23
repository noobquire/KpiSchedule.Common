namespace KpiSchedule.Common.Entities
{
    /// <summary>
    /// Subject DB entity.
    /// </summary>
    public class SubjectEntity
    {
        /// <summary>
        /// Subject name as displayed in schedule.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Subject full name if it is different from <see cref="SubjectName"/>.
        /// Null if the same as <see cref="SubjectName"/>.
        /// </summary>
        public string SubjectFullName { get; set; }
    }
}
