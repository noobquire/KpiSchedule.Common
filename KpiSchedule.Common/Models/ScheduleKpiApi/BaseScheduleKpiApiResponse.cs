namespace KpiSchedule.Common.Models.ScheduleKpiApi
{
    /// <summary>
    /// Base model used for responses from schedule.kpi.ua API.
    /// </summary>
    public abstract class BaseScheduleKpiApiResponse<TResponseModel> 
    {
        /// <summary>
        /// Paging information.
        /// </summary>
        public ScheduleKpiApiPaging Paging { get; set; }

        /// <summary>
        /// Response data collection.
        /// </summary>
        public IList<TResponseModel> Data { get; set; }
    }
}
