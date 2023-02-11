﻿using Serilog;
using System.Text.Json;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.RozKpiApi;
using System.Text;
using HtmlAgilityPack;
using static KpiSchedule.Common.Clients.RozKpiApiClientConstants;
using KpiSchedule.Common.Parsers;
using KpiSchedule.Common.Parsers.ScheduleGroupSelection;

namespace KpiSchedule.Common.Clients
{
    /// <summary>
    /// Client used to get academic groups from roz.kpi.ua API.
    /// </summary>
    public class RozKpiApiClient : ClientBase
    {
        private readonly HttpClient client;
        private readonly string formValidationKeyValue;

        /// <summary>
        /// Initialize a new instance of the <see cref="RozKpiApiClient"/> class.
        /// </summary>
        /// <param name="clientFactory">HTTP client factory.</param>
        /// <param name="logger">Logging interface.</param>
        public RozKpiApiClient(IHttpClientFactory clientFactory, ILogger logger) : base(logger)
        {
            client = clientFactory.CreateClient(nameof(RozKpiApiClient));
            client.DefaultRequestHeaders.Host = client.BaseAddress.Host;

            formValidationKeyValue = GetFormEventValidation().Result;
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
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(requestApi, requestContent);

            var teachers = await VerifyAndParseResponseBody<RozKpiApiTeachersList>(response);

            teachers.TeacherNamePrefix = teacherNamePrefix;
            return teachers;
        }

        /// <summary>
        /// Get schedule group selection HTML page.
        /// </summary>
        /// <returns>Schedule group selection HTML page.</returns>
        public async Task<HtmlDocument> GetGroupSelectionPage()
        {
            string requestApi = "ScheduleGroupSelection.aspx";

            var response = await client.GetAsync(requestApi);

            var requestUrl = response.RequestMessage.RequestUri.ToString();
            await CheckIfSuccessfulResponse(response, requestUrl);
            await CheckIfResponseBodyIsNullOrEmpty(response, requestUrl);

            var responseHtml = await response.Content.ReadAsStringAsync();
            var document = new HtmlDocument();
            document.LoadHtml(responseHtml);

            return document;
        }

        private async Task<string> GetFormEventValidation()
        {
            var groupSelectionPage = await GetGroupSelectionPage();

            var parser = new FormValidationParser(groupSelectionPage);

            return parser.Parse();
        } 

        public async Task<HtmlDocument> GetGroupSchedulePage(string groupName)
        {
            string requestApi = "ScheduleGroupSelection.aspx";

            var requestDictionary = new Dictionary<string, string>()
            {
                [FORM_EVENT_VALIDATION_KEY] = formValidationKeyValue,
                [FORM_SHOW_SCHEDULE_KEY] = FORM_SHOW_SCHEDULE_VALUE,
                [FORM_GROUP_NAME_KEY] = groupName,
                [FORM_EVENT_TARGET_KEY] = string.Empty
            };

            var request = new FormUrlEncodedContent(requestDictionary);

            var response = await client.PostAsync(requestApi, request);

            var requestUrl = response.RequestMessage.RequestUri.ToString();
            await CheckIfSuccessfulResponse(response, requestUrl);
            await CheckIfResponseBodyIsNullOrEmpty(response, requestUrl);

            var responseHtml = await response.Content.ReadAsStringAsync();
            var document = new HtmlDocument();
            document.LoadHtml(responseHtml);

            return document;
        }
    }
}
