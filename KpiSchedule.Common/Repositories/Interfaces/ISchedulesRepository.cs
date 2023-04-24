using KpiSchedule.Common.Entities.Base;

namespace KpiSchedule.Common.Repositories.Interfaces
{
    /// <summary>
    /// Base read-only repository interface for getting schedules data from DB.
    /// </summary>
    /// <typeparam name="TSchedule">Schedule type.</typeparam>
    /// <typeparam name="TDay">Schedule day type.</typeparam>
    /// <typeparam name="TPair">Pair type.</typeparam>
    public interface ISchedulesRepository<TSchedule, TDay, TPair> 
        where TSchedule : BaseScheduleEntity<TDay, TPair> 
        where TDay : BaseScheduleDayEntity<TPair> 
        where TPair : BaseSchedulePairEntity
    {
        /// <summary>
        /// Get schedule data.
        /// </summary>
        /// <param name="scheduleId">Schedule unique identifier.</param>
        /// <returns>Schedule data.</returns>
        Task<TSchedule> GetScheduleById(Guid scheduleId);

        /// <summary>
        /// Get list of subjects in a schedule.
        /// </summary>
        /// <param name="scheduleId">Schedule unique identifier.</param>
        /// <returns>List of subjects.</returns>
        Task<IEnumerable<SubjectEntity>> GetScheduleSubjects(Guid scheduleId);

        /// <summary>
        /// Write schedule to DB.
        /// </summary>
        /// <param name="schedule">Schedule entity.</param>
        /// <returns>Task.</returns>
        Task PutSchedule(TSchedule schedule);

        /// <summary>
        /// Batch write schedules to DB.
        /// </summary>
        /// <param name="schedules">Schedule entities.</param>
        /// <returns>Task.</returns>
        Task BatchPutSchedules(IEnumerable<TSchedule> schedules);

        /// <summary>
        /// Hard delete schedule by removing it from DB.
        /// </summary>
        /// <param name="scheduleId">Schedule unique identifier.</param>
        /// <returns>Task.</returns>
        Task DeleteSchedule(Guid scheduleId);
    }
}
