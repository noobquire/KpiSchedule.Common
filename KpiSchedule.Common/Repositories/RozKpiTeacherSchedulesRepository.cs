using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Repositories
{
    public class RozKpiTeacherSchedulesRepository : BaseDynamoDbSchedulesRepository<RozKpiApiTeacherSchedule>
    {
        public RozKpiTeacherSchedulesRepository(IDynamoDBContext dynamoDbContext) : base(dynamoDbContext)
        {
        }

        public async Task<IEnumerable<TeacherScheduleSearchResult>> SearchScheduleId(string searchQuery)
        {
            var query = new ScanCondition("teacherName", ScanOperator.BeginsWith, searchQuery);
            var results = await dynamoDbContext.ScanAsync<TeacherScheduleSearchResult>(new[] { query }).GetRemainingAsync();
            return results;
        }
    }
}
