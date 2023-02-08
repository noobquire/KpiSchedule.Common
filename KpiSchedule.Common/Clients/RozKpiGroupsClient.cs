using KpiSchedule.Common.Models;
using Serilog;
using System.Text.Json;
using KpiSchedule.Common.Exceptions;

namespace KpiSchedule.Common.Clients
{
    /// <summary>
    /// Client used to get academic groups from roz.kpi.ua API.
    /// </summary>
    public class RozKpiGroupsClient : ClientBase
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initialize a new instance of the <see cref="RozKpiGroupsClient"/> class.
        /// </summary>
        /// <param name="clientFactory">HTTP client factory.</param>
        /// <param name="logger">Logging interface.</param>
        public RozKpiGroupsClient(IHttpClientFactory clientFactory, ILogger logger) : base(logger)
        {
            this.client = clientFactory.CreateClient(nameof(RozKpiGroupsClient));
        }

        /// <summary>
        /// Get list of group names starting with specified prefix.
        /// </summary>
        /// <param name="groupPrefix">Group prefix.</param>
        /// <returns>List of groups with specified prefix.</returns>
        /// <exception cref="KpiScheduleClientException">Unable to deserialize response.</exception>
        public async Task<RozKpiApiGroupsList> GetGroups(string groupPrefix)
        {
            string requestApi = "/GetGroups";
            var request = new BaseRozKpiApiRequest(groupPrefix);
            var requestJson = JsonSerializer.Serialize(request);
            var requestContent = new StringContent(requestJson);

            var response = await client.PostAsync(requestApi, requestContent);

            await CheckIfSuccessfulResponse(response, requestApi);
            await CheckIfResponseBodyIsNullOrEmpty(response, requestApi);

            var responseJson = await response.Content.ReadAsStringAsync();

            var groups = new RozKpiApiGroupsList();
            try
            {
                groups = JsonSerializer.Deserialize<RozKpiApiGroupsList>(responseJson);
            }
            catch (JsonException ex)
            {
                HandleNonSerializableResponse<RozKpiApiGroupsList>(responseJson, ex);
            }

            groups!.GroupPrefix = groupPrefix;
            return groups;
        }
    }
}
