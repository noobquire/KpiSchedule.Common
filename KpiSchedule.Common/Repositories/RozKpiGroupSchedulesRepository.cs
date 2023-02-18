using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using KpiSchedule.Common.Entities.RozKpi;

namespace KpiSchedule.Common.Repositories
{
    public class RozKpiGroupSchedulesRepository : BaseDynamoDbSchedulesRepository<GroupScheduleEntity>
    {
        public RozKpiGroupSchedulesRepository(IDynamoDBContext dynamoDbContext) : base(dynamoDbContext)
        {
        }

        public async Task<IEnumerable<GroupScheduleSearchResult>> SearchScheduleId(string searchQuery)
        {
            var query = new ScanCondition("groupName", ScanOperator.BeginsWith, searchQuery);
            var results = await dynamoDbContext.ScanAsync<GroupScheduleSearchResult>(new[] { query }).GetRemainingAsync();
            return results;
        }
    }
}
