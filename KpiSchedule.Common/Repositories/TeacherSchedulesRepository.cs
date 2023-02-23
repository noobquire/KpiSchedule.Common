using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using KpiSchedule.Common.Entities;
using KpiSchedule.Common.Repositories.Interfaces;

namespace KpiSchedule.Common.Repositories
{
    /// <summary>
    /// Teacher schedules repository that wraps roz.kpi.ua teacher schedules extracted via ETL.
    /// </summary>
    public class TeacherSchedulesRepository : BaseDynamoDbSchedulesRepository<TeacherScheduleEntity, TeacherScheduleDayEntity, TeacherSchedulePairEntity>, ITeacherSchedulesRepository
    {

        /// <summary>
        /// Initialize a new instance of the <see cref="TeacherSchedulesRepository"/> class.
        /// </summary>
        public TeacherSchedulesRepository(IDynamoDBContext dynamoDbContext) : base(dynamoDbContext)
        {
        }

        /// <inheritdoc/>
        public async Task<TeacherScheduleEntity> GetScheduleById(Guid scheduleId)
        {
            var schedule = await dynamoDbContext.LoadAsync<TeacherScheduleEntity>(scheduleId);
            return schedule;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SubjectEntity>> GetScheduleSubjects(Guid scheduleId)
        {
            var schedule = await GetScheduleById(scheduleId);

            var firstWeekSubjects = schedule.FirstWeek.SelectMany(d => d.Pairs).Select(p => p.Subject);
            var secondWeekSubjects = schedule.SecondWeek.SelectMany(d => d.Pairs).Select(p => p.Subject);

            var allSubjects = firstWeekSubjects.Concat(secondWeekSubjects);
            var uniqueSubjects = allSubjects.DistinctBy(s => s.SubjectName);

            return uniqueSubjects;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TeacherScheduleSearchResult>> SearchTeacherSchedules(string teacherNamePrefix)
        {
            var query = new ScanCondition("TeacherName", ScanOperator.BeginsWith, teacherNamePrefix);
            var results = await dynamoDbContext.ScanAsync<TeacherScheduleSearchResult>(new[] { query }).GetRemainingAsync();
            return results;
        }
    }
}
