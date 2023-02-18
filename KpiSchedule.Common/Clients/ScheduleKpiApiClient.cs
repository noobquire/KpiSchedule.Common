using Serilog;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.ScheduleKpiApi.Responses;
using KpiSchedule.Common.Clients.Interfaces;

namespace KpiSchedule.Common.Clients
{
    /// <summary>
    /// Client for pulling schedules data from schedule.kpi.ua.
    /// </summary>
    public class ScheduleKpiApiClient : BaseClient, IScheduleKpiApiClient
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

        /// <inheritdoc/>
        public async Task<ScheduleKpiApiGroupsResponse> GetAllGroups()
        {
            string requestApi = "schedule/groups";

            var response = await client.GetAsync(requestApi);
            var groups = await VerifyAndParseResponseBody<ScheduleKpiApiGroupsResponse>(response);

            return groups;
        }

        /// <inheritdoc/>
        public async Task<ScheduleKpiApiTeachersResponse> GetAllTeachers()
        {
            string requestApi = "schedule/lecturer/list";

            var response = await client.GetAsync(requestApi);
            var teachers = await VerifyAndParseResponseBody<ScheduleKpiApiTeachersResponse>(response);

            return teachers;
        }

        /// <inheritdoc/>
        public async Task<ScheduleKpiApiGroupScheduleResponse> GetGroupSchedule(string groupId)
        {
            string requestApi = $"schedule/lessons/?groupId={groupId}";

            var response = await client.GetAsync(requestApi);
            var schedule = await VerifyAndParseResponseBody<ScheduleKpiApiGroupScheduleResponse>(response);

            return schedule;
        }

        /// <inheritdoc/>
        public async Task<ScheduleKpiApiTimeResponse> GetTimeInfo()
        {
            string requestApi = "time/current";

            var response = await client.GetAsync(requestApi);
            var timeInfo = await VerifyAndParseResponseBody<ScheduleKpiApiTimeResponse>(response);

            return timeInfo;
        }

        /// <inheritdoc/>
        public async Task<ScheduleKpiApiTeacherScheduleResponse> GetTeacherSchedule(string teacherId)
        {
            string requestApi = $"schedule/lecturer?lecturerId={teacherId}";

            var response = await client.GetAsync(requestApi);
            var schedule = await VerifyAndParseResponseBody<ScheduleKpiApiTeacherScheduleResponse>(response);

            return schedule;
        }

        // TODO: Get exam schedule /exams/group?groupId=id (no way to get actual response now, reverse engineer frontend app)
        // https://github.com/kpi-ua/schedule.kpi.ua/blob/master/src/components/examComponent/exam.jsx#L9
    }
}
