using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities
{
    /// <summary>
    /// Group schedule DB entity.
    /// </summary>
    [DynamoDBTable("KpiSchedule-GroupSchedules")]
    public class GroupScheduleEntity : BaseScheduleEntity<GroupScheduleDayEntity, GroupSchedulePairEntity>
    {
        /// <summary>
        /// Group name.
        /// </summary>
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string GroupName { get; set; }

        [DynamoDBHashKey]
        public override Guid ScheduleId { get; set; }
    }
}
