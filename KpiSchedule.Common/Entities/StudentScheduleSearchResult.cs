using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities
{
    [DynamoDBTable("KpiSchedule-StudentSchedules")]
    public class StudentScheduleSearchResult
    {
        /// <summary>
        /// Schedule unique identifier.
        /// </summary>
        [DynamoDBHashKey]
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// Owning user unique identifier.
        /// </summary>
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string OwnerId { get; set; }

        /// <summary>
        /// Last updated UTC timestamp.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Boolean indicating if this schedule should be made public to other users.
        /// </summary>
        public bool IsPublic { get; set; }
    }
}
