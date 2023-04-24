namespace KpiSchedule.Common.Models.ScheduleKpiApi.Base
{
    /// <summary>
    /// Paging information in responses from schedule.kpi.ua API.
    /// Pagination query parameters do not seem work with this API, so there is always only one page.
    /// </summary>
    public class ScheduleKpiApiPaging
    {
        /// <summary>
        /// Current page number.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Total page count.
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Count of items on current page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Boolean indicating if current page has previous page.
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Boolean indicating if current page has next page.
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Boolean indicating if current page is first page.
        /// </summary>
        public bool IsFirstPage { get; set; }

        /// <summary>
        /// Boolean indicating if current page is last page.
        /// </summary>
        public bool IsLastPage { get; set; }

        /// <summary>
        /// 1-based number of first item on current page.
        /// </summary>
        public int FirstItemOnPage { get; set; }

        /// <summary>
        /// 1-based number of last item on current page.
        /// </summary>
        public int LastItemOnPage { get; set; }
    }
}
