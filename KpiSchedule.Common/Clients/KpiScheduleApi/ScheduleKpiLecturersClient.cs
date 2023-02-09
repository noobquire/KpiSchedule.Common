using Serilog;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.ScheduleKpiApi.Responses;

namespace KpiSchedule.Common.Clients.RozKpiApi
{
    /// <summary>
    /// Client used to get lecturers/teachers from schedule.kpi.ua API.
    /// </summary>
    public class ScheduleKpiLecturersClient : ClientBase
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initialize a new instance of the <see cref="ScheduleKpiLecturersClient"/> class.
        /// </summary>
        /// <param name="clientFactory">HTTP client factory.</param>
        /// <param name="logger">Logging interface.</param>
        public ScheduleKpiLecturersClient(IHttpClientFactory clientFactory, ILogger logger) : base(logger)
        {
            client = clientFactory.CreateClient(nameof(ScheduleKpiLecturersClient));
        }

        /// <summary>
        /// Get list of all lecturers.
        /// </summary>
        /// <returns>List of all groups.</returns>
        /// <exception cref="KpiScheduleClientException">Unable to deserialize response.</exception>
        public async Task<ScheduleKpiApiLecturersResponse> GetAllLecturers()
        {
            string requestApi = "list";

            var response = await client.GetAsync(requestApi);
            var lecturers = await VerifyAndParseResponseBody<ScheduleKpiApiLecturersResponse>(response);

            return lecturers;
        }
    }
}
