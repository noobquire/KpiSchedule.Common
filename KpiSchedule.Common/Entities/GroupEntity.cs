namespace KpiSchedule.Common.Entities
{
    /// <summary>
    /// Group DB entity.
    /// </summary>
    public class GroupEntity
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Group schedule unique identifier.
        /// </summary>
        public Guid ScheduleId { get; set; }
    }
}
