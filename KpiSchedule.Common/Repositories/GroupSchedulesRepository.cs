using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using KpiSchedule.Common.Entities.Group;
using KpiSchedule.Common.Entities.Teacher;
using KpiSchedule.Common.Repositories.Interfaces;

namespace KpiSchedule.Common.Repositories
{
    /// <summary>
    /// Group schedules repository that wraps roz.kpi.ua group schedules extracted via ETL.
    /// </summary>
    public class GroupSchedulesRepository : BaseDynamoDbSchedulesRepository<GroupScheduleEntity, GroupScheduleDayEntity, GroupSchedulePairEntity>, IGroupSchedulesRepository
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="GroupSchedulesRepository"/> class.
        /// </summary>
        /// <param name="dynamoDbContext">DynamoDB context.</param>
        public GroupSchedulesRepository(IDynamoDBContext dynamoDbContext) : base(dynamoDbContext)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<GroupScheduleSearchResult>> SearchGroupSchedules(string groupNamePrefix)
        {
            var query = new ScanCondition("GroupName", ScanOperator.BeginsWith, groupNamePrefix);
            var results = await dynamoDbContext.ScanAsync<GroupScheduleSearchResult>(new[] { query }).GetRemainingAsync();
            return results;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TeacherEntity>> GetTeachersInGroupSchedule(Guid groupScheduleId)
        {
            var schedule = await GetScheduleById(groupScheduleId);
            CheckIfScheduleIsNull(schedule);

            var firstWeekTeachers = schedule.FirstWeek.SelectMany(d => d.Pairs).SelectMany(p => p.Teachers);
            var secondWeekTeachers = schedule.SecondWeek.SelectMany(d => d.Pairs).SelectMany(p => p.Teachers);

            var allTeachers = firstWeekTeachers.Concat(secondWeekTeachers);
            var uniqueTeachers = allTeachers.DistinctBy(s => s.ScheduleId);

            return uniqueTeachers;
        }
    }
}
