using Serilog;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.ScheduleKpiApi.Responses;

namespace KpiSchedule.Common.Clients
{
    /// <summary>
    /// Client used to get academic groups from schedule.kpi.ua API.
    /// </summary>
    public class ScheduleKpiApiClient : ClientBase
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initialize a new instance of the <see cref="ScheduleKpiApiClient"/> class.
        /// </summary>
        /// <param name="clientFactory">HTTP client factory.</param>
        /// <param name="logger">Logging interface.</param>
        public ScheduleKpiApiClient(IHttpClientFactory clientFactory, ILogger logger) : base(logger)
        {
            client = clientFactory.CreateClient(nameof(ScheduleKpiApiClient));
        }

        /// <summary>
        /// Get list of all groups.
        /// </summary>
        /// <returns>List of all groups.</returns>
        /// <exception cref="KpiApiClientException">Unable to deserialize response.</exception>
        public async Task<ScheduleKpiApiGroupsResponse> GetAllGroups()
        {
            string requestApi = "schedule/groups";

            var response = await client.GetAsync(requestApi);
            var groups = await VerifyAndParseResponseBody<ScheduleKpiApiGroupsResponse>(response);

            return groups;
        }

        /// <summary>
        /// Get list of all lecturers.
        /// </summary>
        /// <returns>List of all groups.</returns>
        /// <exception cref="KpiApiClientException">Unable to deserialize response.</exception>
        public async Task<ScheduleKpiApiLecturersResponse> GetAllLecturers()
        {
            string requestApi = "schedule/lecturer/list";

            var response = await client.GetAsync(requestApi);
            var lecturers = await VerifyAndParseResponseBody<ScheduleKpiApiLecturersResponse>(response);

            return lecturers;
        }

        /// <summary>
        /// Get group schedule for specific group.
        /// </summary>
        /// <param name="groupId">Unique group identifier.</param>
        /// <returns>Group schedule.</returns>
        public async Task<ScheduleKpiApiGroupScheduleResponse> GetGroupSchedule(string groupId)
        {
            string requestApi = $"schedule/lessons/?groupId={groupId}";

            var response = await client.GetAsync(requestApi);
            var schedule = await VerifyAndParseResponseBody<ScheduleKpiApiGroupScheduleResponse>(response);

            return schedule;
        }

        /// <summary>
        /// Get schedule current time info.
        /// </summary>
        /// <returns>Time info.</returns>
        public async Task<ScheduleKpiApiTimeResponse> GetTimeInfo()
        {
            string requestApi = "time/current";

            var response = await client.GetAsync(requestApi);
            var timeInfo = await VerifyAndParseResponseBody<ScheduleKpiApiTimeResponse>(response);

            return timeInfo;
        }
    }
}
