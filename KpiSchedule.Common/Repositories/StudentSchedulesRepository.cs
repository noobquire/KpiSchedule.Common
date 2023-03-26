using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using KpiSchedule.Common.Entities;
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

        public override async Task<StudentScheduleEntity> GetScheduleById(Guid scheduleId)
        {
            var results = await dynamoDbContext.QueryAsync<StudentScheduleEntity>(scheduleId).GetRemainingAsync();
            var schedule = results.FirstOrDefault();
            return schedule;
        }

        public async Task<IEnumerable<StudentScheduleEntity>> GetSchedulesForStudent(Guid userId)
        {
            var schedulesQuery = new QueryCondition("OwnerId", QueryOperator.Equal, userId);
            var results = await dynamoDbContext.QueryAsync<StudentScheduleEntity>(new[] { schedulesQuery }).GetRemainingAsync();
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
