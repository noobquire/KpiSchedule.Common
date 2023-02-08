using Serilog;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.ScheduleKpiApi;

namespace KpiSchedule.Common.Clients.RozKpiApi
{
    /// <summary>
    /// Client used to get academic groups from schedule.kpi.ua API.
    /// </summary>
    public class ScheduleKpiGroupsClient : ClientBase
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initialize a new instance of the <see cref="ScheduleKpiGroupsClient"/> class.
        /// </summary>
        /// <param name="clientFactory">HTTP client factory.</param>
        /// <param name="logger">Logging interface.</param>
        public ScheduleKpiGroupsClient(IHttpClientFactory clientFactory, ILogger logger) : base(logger)
        {
            client = clientFactory.CreateClient(nameof(ScheduleKpiGroupsClient));
        }

        /// <summary>
        /// Get list of all groups.
        /// </summary>
        /// <returns>List of all groups.</returns>
        /// <exception cref="KpiScheduleClientException">Unable to deserialize response.</exception>
        public async Task<ScheduleKpiApiGroupsResponse> GetAllGroups()
        {
            string requestApi = "groups";

            var response = await client.GetAsync(requestApi);
            var groups = await VerifyAndParseResponseBody<ScheduleKpiApiGroupsResponse>(response);

            return groups;
        }
    }
}
