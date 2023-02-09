namespace KpiSchedule.Common.Models.ScheduleKpiApi.Responses
{
    /// <summary>
    /// Base model used for responses from schedule.kpi.ua API.
    /// </summary>
    public abstract class BaseScheduleKpiApiResponse<TResponseModel>
    {
        /// <summary>
        /// Paging information.
        /// </summary>
        public virtual ScheduleKpiApiPaging Paging { get; set; }

        /// <summary>
        /// Response data collection.
        /// </summary>
        public virtual IList<TResponseModel> Data { get; set; }
    }
}
