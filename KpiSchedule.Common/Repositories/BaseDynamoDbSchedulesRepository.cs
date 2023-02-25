using Amazon.DynamoDBv2.DataModel;
using KpiSchedule.Common.Entities;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Repositories.Interfaces;

namespace KpiSchedule.Common.Repositories
{
    public abstract class BaseDynamoDbSchedulesRepository<TSchedule, TDay, TPair> : ISchedulesRepository<TSchedule, TDay, TPair> 
        where TSchedule : BaseScheduleEntity<TDay, TPair>
        where TDay : BaseScheduleDayEntity<TPair>
        where TPair : BaseSchedulePairEntity
    {
        protected readonly IDynamoDBContext dynamoDbContext;

        protected BaseDynamoDbSchedulesRepository(IDynamoDBContext dynamoDbContext)
        {
            this.dynamoDbContext = dynamoDbContext;
        }

        private void AssignNullRoomsToEmptyArrays(IEnumerable<TDay> week)
        {
            foreach(var day in week)
            {
                foreach(var pair in day.Pairs)
                {
                    pair.Rooms ??= Enumerable.Empty<string>().ToList();
                }
            }
        }

        public virtual async Task<TSchedule> GetScheduleById(Guid scheduleId)
        {
            var schedule = await dynamoDbContext.LoadAsync<TSchedule>(scheduleId);
            // this is a crutch, idk why empty room arrays are saved as null in dynamodb
            AssignNullRoomsToEmptyArrays(schedule.FirstWeek);
            AssignNullRoomsToEmptyArrays(schedule.SecondWeek);

            return schedule;
        }

        public virtual async Task PutSchedule(TSchedule schedule)
        {
            if(schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }
            schedule.UpdatedAt = DateTime.UtcNow;
            await dynamoDbContext.SaveAsync(schedule);
        }

        public virtual async Task BatchPutSchedules(IEnumerable<TSchedule> schedules)
        {
            if(!schedules.Any())
            {
                throw new ArgumentException("Input contains no elements.", nameof(schedules));
            }
            
            foreach(var schedule in schedules)
            {
                schedule.UpdatedAt = DateTime.UtcNow;
            }

            var writeSchedules = dynamoDbContext.CreateBatchWrite<TSchedule>();
            writeSchedules.AddPutItems(schedules);

            await writeSchedules.ExecuteAsync();
        }

        public virtual async Task<IEnumerable<SubjectEntity>> GetScheduleSubjects(Guid scheduleId)
        {
            var schedule = await GetScheduleById(scheduleId);
            CheckIfScheduleIsNull(schedule);

            var firstWeekSubjects = schedule.FirstWeek.SelectMany(d => d.Pairs).Select(p => p.Subject);
            var secondWeekSubjects = schedule.SecondWeek.SelectMany(d => d.Pairs).Select(p => p.Subject);

            var allSubjects = firstWeekSubjects.Concat(secondWeekSubjects);
            var uniqueSubjects = allSubjects.DistinctBy(s => s.SubjectName);

            return uniqueSubjects;
        }

        public async Task DeleteSchedule(Guid scheduleId)
        {
            var schedule = await GetScheduleById(scheduleId);
            CheckIfScheduleIsNull(schedule);

            await dynamoDbContext.DeleteAsync(scheduleId);
        }

        protected void CheckIfScheduleIsNull(TSchedule schedule)
        {
            if (schedule is null)
            {
                throw new ScheduleNotFoundException();
            }
        }
    }
}
