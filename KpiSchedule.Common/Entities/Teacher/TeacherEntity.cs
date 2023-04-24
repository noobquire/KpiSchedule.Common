namespace KpiSchedule.Common.Entities.Teacher
{
    /// <summary>
    /// Teacher DB entity.
    /// </summary>
    public class TeacherEntity
    {
        /// <summary>
        /// Teacher name as displayed in schedule.
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// Teacher full name if it is different from <see cref="TeacherName"/>.
        /// Null if the same as <see cref="TeacherName"/>.
        /// </summary>
        public string TeacherFullName { get; set; }

        /// <summary>
        /// Teacher schedule unique identifier.
        /// </summary>
        public Guid ScheduleId { get; set; }
    }
}
