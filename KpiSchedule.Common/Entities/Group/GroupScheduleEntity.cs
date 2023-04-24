using Amazon.DynamoDBv2.DataModel;
using KpiSchedule.Common.Entities.Base;

namespace KpiSchedule.Common.Entities.Group
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
