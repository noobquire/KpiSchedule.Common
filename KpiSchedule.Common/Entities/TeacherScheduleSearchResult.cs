using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Entities
{
    [DynamoDBTable("RozKpiTeacherSchedules")]
    public class TeacherScheduleSearchResult
    {
        [DynamoDBHashKey]
        public Guid scheduleId { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string teacherName { get; set; }
    }
}
