using KpiSchedule.Common.Models;
using Serilog;
using System.Text.Json;

namespace KpiSchedule.Common.Clients
{
    public class KpiGroupsClient : ClientBase
    {
        private readonly HttpClient client;
        private readonly ILogger logger;

        public KpiGroupsClient(HttpClient client, ILogger logger) : base(logger)
        {
            this.client = client;
            this.logger = logger;
        }

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
