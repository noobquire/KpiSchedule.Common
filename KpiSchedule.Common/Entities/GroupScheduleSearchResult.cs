using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities
{
    /// <summary>
    /// Search results entity for group schedules.
    /// </summary>
    [DynamoDBTable("KpiSchedule-GroupSchedules")]
    public class GroupScheduleSearchResult
    {
        /// <summary>
        /// Schedule unique identifier.
        /// </summary>
        [DynamoDBHashKey]
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// Group name.
        /// </summary>
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string GroupName { get; set; }

        /// <summary>
        /// Last updated UTC timestamp.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
