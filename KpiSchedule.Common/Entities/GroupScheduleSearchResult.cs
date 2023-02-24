using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities
{
    [DynamoDBTable("KpiSchedule-GroupSchedules", LowerCamelCaseProperties = true)]
    public class GroupScheduleSearchResult
    {
        [DynamoDBHashKey]
        public Guid ScheduleId { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string GroupName { get; set; }
    }
}
