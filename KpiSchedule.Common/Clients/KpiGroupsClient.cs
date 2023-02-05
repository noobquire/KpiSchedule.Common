using KpiSchedule.Common.Models;
using Serilog;
using System.Text.Json;

namespace KpiSchedule.Common.Clients
{
    /// <summary>
    /// Client used to get academic groups from KPI API.
    /// </summary>
    public class KpiGroupsClient : ClientBase
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initialize a new instance of the <see cref="KpiGroupsClient"/> class.
        /// </summary>
        /// <param name="client">HTTP client.</param>
        /// <param name="logger">Logging interface.</param>
        public KpiGroupsClient(HttpClient client, ILogger logger) : base(logger)
        {
            this.client = client;
        }
        
        /// <summary>
        /// Get list of group names starting with specified prefix.
        /// </summary>
        /// <param name="groupPrefix">Group prefix.</param>
        /// <returns></returns>
        public async Task<KpiApiGroupsList> GetGroups(string groupPrefix)
        {
            string requestApi = "/GetGroups";
            var request = new BaseKpiApiRequest(groupPrefix);
            var requestJson = JsonSerializer.Serialize(request);
            var requestContent = new StringContent(requestJson);

            var response = await client.PostAsync(requestApi, requestContent);

            await CheckIfSuccessfulResponse(response, requestApi);
            await CheckIfResponseBodyIsNullOrEmpty(response, requestApi);

            var responseJson = await response.Content.ReadAsStringAsync();
            var groups = JsonSerializer.Deserialize<KpiApiGroupsList>(responseJson);

            return groups!;
        }
    }
}
