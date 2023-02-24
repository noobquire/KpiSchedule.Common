using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities
{
    [DynamoDBTable("KpiSchedule-TeacherSchedules")]
    public class TeacherScheduleSearchResult
    {
        [DynamoDBHashKey]
        public Guid ScheduleId { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string TeacherName { get; set; }
    }
}
