using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities
{
    [DynamoDBTable("RozKpiGroupSchedules", LowerCamelCaseProperties = true)]
    public class GroupScheduleSearchResult
    {
        [DynamoDBHashKey]
        public Guid scheduleId { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string groupName { get; set; }
    }
}
