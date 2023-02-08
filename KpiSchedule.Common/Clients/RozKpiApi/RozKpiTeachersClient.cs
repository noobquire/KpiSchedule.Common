using Serilog;
using System.Text.Json;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Clients.RozKpiApi
{
    /// <summary>
    /// Client used to get teachers from roz.kpi.ua API.
    /// </summary>
    public class RozKpiTeachersClient : ClientBase
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initialize a new instance of the <see cref="RozKpiTeachersClient"/> class.
        /// </summary>
        /// <param name="clientFactory">HTTP client factory.</param>
        /// <param name="logger">Logging interface.</param>
        public RozKpiTeachersClient(IHttpClientFactory clientFactory, ILogger logger) : base(logger)
        {
            client = clientFactory.CreateClient(nameof(RozKpiTeachersClient));
        }

        /// <summary>
        /// Get list of teacher names starting with specified prefix.
        /// </summary>
        /// <param name="teacherNamePrefix">Teacher name prefix.</param>
        /// <returns>List of teachers with specified name prefix.</returns>
        /// <exception cref="KpiScheduleClientException">Unable to deserialize response.</exception>
        public async Task<RozKpiApiTeachersList> GetTeacher(string teacherNamePrefix)
        {
            string requestApi = "GetLecturers";
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
