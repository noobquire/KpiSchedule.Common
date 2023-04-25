using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using KpiSchedule.Common.Entities.Base;
using KpiSchedule.Common.Entities.Student;
using KpiSchedule.Common.Models;
using KpiSchedule.Common.Repositories.Interfaces;

namespace KpiSchedule.Common.Repositories
{
    /// <summary>
    /// Student schedules repository which reads and writes student schedule data to/from DynamoDB.
    /// </summary>
    public class StudentSchedulesRepository : BaseDynamoDbSchedulesRepository<StudentScheduleEntity, StudentScheduleDayEntity, StudentSchedulePairEntity>, IStudentSchedulesRepository
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="TeacherSchedulesRepository"/> class.
        /// </summary>
        /// <param name="dynamoDbContext">DynamoDbContext.</param>
        public StudentSchedulesRepository(IDynamoDBContext dynamoDbContext) : base(dynamoDbContext)
        {
        }

        public async Task DeletePair(Guid scheduleId, PairIdentifier pairId)
        {
            var schedule = await GetScheduleById(scheduleId);
            CheckIfScheduleIsNull(schedule);

            schedule.RemoveSchedulePair(pairId);

            await PutSchedule(schedule);
        }

        public async Task<IEnumerable<StudentScheduleSearchResult>> GetSchedulesForStudent(string userId)
        {
            var query = new ScanCondition("OwnerId", ScanOperator.Equal, userId);
            var results = await dynamoDbContext.ScanAsync<StudentScheduleSearchResult>(new[] { query }).GetRemainingAsync();
            return results;
        }

        public async Task<StudentScheduleEntity> UpdatePair(Guid scheduleId, PairIdentifier pairId, StudentSchedulePairEntity pair)
        {
            var schedule = await GetScheduleById(scheduleId);
            CheckIfScheduleIsNull(schedule);

            schedule.UpdateSchedulePair(pairId, pair);

            await PutSchedule(schedule);

            return schedule;
        }
    }
}
