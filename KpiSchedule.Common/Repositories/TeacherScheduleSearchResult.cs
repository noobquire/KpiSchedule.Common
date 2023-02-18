using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Repositories
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
