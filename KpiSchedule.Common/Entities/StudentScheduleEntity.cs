using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities
{
    /// <summary>
    /// DB entity for personal student schedule.
    /// </summary>
    [DynamoDBTable("KpiSchedule-StudentSchedules")]
    public class StudentScheduleEntity : BaseScheduleEntity<StudentScheduleDayEntity, StudentSchedulePairEntity>
    {
        /// <summary>
        /// Boolean indicating if this schedule should be made public to other users.
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Id of the user who owns this schedule.
        /// </summary>
        [DynamoDBHashKey]
        public string OwnerId { get; set; }

        /// <summary>
        /// Id of the schedule.
        /// </summary>
        [DynamoDBRangeKey]
        public override Guid ScheduleId { get; set; }
    }
}
