using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities.RozKpi
{
    [DynamoDBTable("RozKpiGroupSchedules", LowerCamelCaseProperties = true)]
    public class GroupScheduleEntity
    {
        [DynamoDBHashKey]
        public Guid ScheduleId { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string GroupName { get; set; }

        [DynamoDBProperty]
        public List<GroupScheduleDayEntity> FirstWeek { get; set; }

        [DynamoDBProperty]
        public List<GroupScheduleDayEntity> SecondWeek { get; set; }
    }
}
