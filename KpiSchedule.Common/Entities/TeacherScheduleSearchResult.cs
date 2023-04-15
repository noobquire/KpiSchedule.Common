using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities
{
    [DynamoDBTable("KpiSchedule-TeacherSchedules")]
    public class TeacherScheduleSearchResult
    {
        /// <summary>
        /// Schedule unique identifier.
        /// </summary>
        [DynamoDBHashKey]
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// Teacher name.
        /// </summary>
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string TeacherName { get; set; }

        /// <summary>
        /// Last updated UTC timestamp.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
