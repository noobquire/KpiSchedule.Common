using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using KpiSchedule.Common.Entities.RozKpi;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Repositories
{
    public class RozKpiGroupSchedulesRepository : BaseDynamoDbSchedulesRepository<GroupScheduleEntity>
    {
        public RozKpiGroupSchedulesRepository(IDynamoDBContext dynamoDbContext) : base(dynamoDbContext)
        {
        }

        public async Task<IEnumerable<GroupScheduleSearchResult>> SearchScheduleId(string searchQuery)
        {
            var query = new QueryCondition("groupName", QueryOperator.BeginsWith, searchQuery);
            var results = await dynamoDbContext.QueryAsync<GroupScheduleSearchResult>(query).GetRemainingAsync();
            return results;
        }
    }
}
