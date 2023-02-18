using Amazon.DynamoDBv2.DataModel;

namespace KpiSchedule.Common.Repositories
{
    public abstract class BaseDynamoDbSchedulesRepository<TSchedule>
    {
        protected readonly IDynamoDBContext dynamoDbContext;

        protected BaseDynamoDbSchedulesRepository(IDynamoDBContext dynamoDbContext)
        {
            this.dynamoDbContext = dynamoDbContext;
        }

        public async Task<TSchedule> GetScheduleById(Guid scheduleId)
        {
            var schedule = await dynamoDbContext.LoadAsync<TSchedule>(scheduleId);
            return schedule;
        }

        public async Task PutSchedule(TSchedule schedule)
        {
            if(schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            await dynamoDbContext.SaveAsync(schedule);
        }

        public async Task BatchPutSchedules(IEnumerable<TSchedule> schedules)
        {
            if(!schedules.Any())
            {
                throw new ArgumentException("Input contains no elements.", nameof(schedules));
            }

            var writeSchedules = dynamoDbContext.CreateBatchWrite<TSchedule>();
            writeSchedules.AddPutItems(schedules);

            await writeSchedules.ExecuteAsync();
        }
    }
}
