using Serilog;
using System.Text.Json;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.RozKpiApi;
using System.Text;

namespace KpiSchedule.Common.Clients
{
    /// <summary>
    /// Client used to get academic groups from roz.kpi.ua API.
    /// </summary>
    public class RozKpiApiClient : ClientBase
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initialize a new instance of the <see cref="RozKpiApiClient"/> class.
        /// </summary>
        /// <param name="clientFactory">HTTP client factory.</param>
        /// <param name="logger">Logging interface.</param>
        public RozKpiApiClient(IHttpClientFactory clientFactory, ILogger logger) : base(logger)
        {
            client = clientFactory.CreateClient(nameof(RozKpiApiClient));
        }

        /// <summary>
        /// Get list of group names starting with specified prefix.
        /// </summary>
        /// <param name="groupPrefix">Group prefix.</param>
        /// <returns>List of groups with specified prefix.</returns>
        /// <exception cref="KpiApiClientException">Unable to deserialize response.</exception>
        public async Task<RozKpiApiGroupsList> GetGroups(string groupPrefix)
        {
            string requestApi = "ScheduleGroupSelection.aspx/GetGroups";
            var request = new BaseRozKpiApiRequest(groupPrefix);
            var requestJson = JsonSerializer.Serialize(request);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Host = client.BaseAddress.Host;

            var response = await client.PostAsync(requestApi, requestContent);

            var groups = await VerifyAndParseResponseBody<RozKpiApiGroupsList>(response);

            groups.GroupPrefix = groupPrefix;
            return groups;
        }

        /// <summary>
        /// Get list of teacher names starting with specified prefix.
        /// </summary>
        /// <param name="teacherNamePrefix">Teacher name prefix.</param>
        /// <returns>List of teachers with specified name prefix.</returns>
        /// <exception cref="KpiApiClientException">Unable to deserialize response.</exception>
        public async Task<RozKpiApiTeachersList> GetTeachers(string teacherNamePrefix)
        {
            string requestApi = "LecturerSelection.aspx/GetLecturers";
            var request = new BaseRozKpiApiRequest(teacherNamePrefix);
            var requestJson = JsonSerializer.Serialize(request);
            var requestContent = new StringContent(requestJson);

            var response = await client.PostAsync(requestApi, requestContent);

            var teachers = await VerifyAndParseResponseBody<RozKpiApiTeachersList>(response);

            teachers.TeacherNamePrefix = teacherNamePrefix;
            return teachers;
        }
    }
}
