using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using KpiSchedule.Common.Entities.RozKpi;

namespace KpiSchedule.Common.Repositories
{
    public class RozKpiTeacherSchedulesRepository : BaseDynamoDbSchedulesRepository<TeacherScheduleEntity>
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
