using Serilog;
using System.Text.Json;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.RozKpiApi;
using System.Text;
using HtmlAgilityPack;
using static KpiSchedule.Common.Clients.RozKpiApiClientConstants;
using KpiSchedule.Common.Parsers.ScheduleGroupSelection;
using System.Net;
using KpiSchedule.Common.Parsers.GroupSchedulePage;

namespace KpiSchedule.Common.Clients
{
    /// <summary>
    /// Client used to get academic groups from roz.kpi.ua API.
    /// </summary>
    public class RozKpiApiGroupsClient : BaseRozKpiApiClient
    {
        private readonly ConflictingGroupNamesParser conflictingGroupNamesParser;
        private readonly GroupSchedulePageParser scheduleParser;

        /// <summary>
        /// Initialize a new instance of the <see cref="RozKpiApiGroupsClient"/> class.
        /// </summary>
        /// <param name="clientFactory">HTTP client factory.</param>
        /// <param name="logger">Logging interface.</param>
        public RozKpiApiGroupsClient(IHttpClientFactory clientFactory,
            ILogger logger,
            FormValidationParser formValidationParser,
            ConflictingGroupNamesParser conflictingGroupNamesParser,
            GroupSchedulePageParser scheduleParser) : base(logger, formValidationParser)
        {
            client = clientFactory.CreateClient(nameof(RozKpiApiGroupsClient));
            client.DefaultRequestHeaders.Host = client.BaseAddress.Host;
            formValidationKeyValue = GetFormEventValidation().Result;
            this.conflictingGroupNamesParser = conflictingGroupNamesParser;
            this.scheduleParser = scheduleParser;
        }

        /// <summary>
        /// Get list of group names starting with specified prefix.
        /// </summary>
        /// <param name="groupPrefix">Group prefix.</param>
        /// <returns>List of groups with specified prefix.</returns>
        /// <exception cref="KpiScheduleClientException">Unable to deserialize response.</exception>
        public async Task<RozKpiApiGroupsList> GetGroups(string groupPrefix)
        {
            string requestApi = "ScheduleGroupSelection.aspx/GetGroups";
            var request = new BaseRozKpiApiRequest(groupPrefix);
            var requestJson = JsonSerializer.Serialize(request);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            logger.Information("Getting group names for prefix {groupPrefix}", groupPrefix);
            var response = await client.PostAsync(requestApi, requestContent);

            var groups = await VerifyAndParseResponseBody<RozKpiApiGroupsList>(response);
            if(groups.Data is null)
            {
                groups.Data = Array.Empty<string>();
            }

            groups.GroupPrefix = groupPrefix;
            return groups;
        }

        /// <summary>
        /// Get group schedule selection HTML page.
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

            return formValidationParser.Parse(groupSelectionPage.DocumentNode);
        }

        /// <summary>
        /// Get group schedule IDs for given group name.
        /// If conflicting group names are found, return all of their IDs.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <returns>Group schedule id, all ids if got group name conflict.</returns>
        public async Task<IEnumerable<Guid>> GetGroupScheduleIds(string groupName)
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

            logger.Information("Getting scheduleId for group {groupName}", groupName);
            var response = await client.PostAsync(requestApi, request);
            logger.Information("Received scheduleId for group {groupName}", groupName);

            var requestUrl = response.RequestMessage.RequestUri.ToString();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // we got group names conflict
                await CheckIfResponseBodyIsNullOrEmpty(response, requestUrl);

                var responseHtml = await response.Content.ReadAsStringAsync();
                var document = new HtmlDocument();
                document.LoadHtml(responseHtml);

                if(IsGroupNotFoundPage(document.DocumentNode))
                {
                    logger.Error("Group {groupName} not found", groupName);
                    throw new KpiScheduleClientGroupNotFoundException("Group with requested name was not found.");
                }

                var conflictingGroups = conflictingGroupNamesParser.Parse(document.DocumentNode);
                return conflictingGroups.Select(group => group.Id);
            }

            await CheckIfExpectedResponseCode(response, HttpStatusCode.Redirect, requestUrl);

            var location = response.Headers.Location.ToString();
            var viewSchedulePrefix = "/Schedules/ViewSchedule.aspx?g=";
            var scheduleIdStr = location.Substring(viewSchedulePrefix.Length);

            return new[] { new Guid(scheduleIdStr) };
        }

        /// <summary>
        /// Get parsed <see cref="RozKpiApiGroupSchedule"/> for given group schedule id.
        /// </summary>
        /// <param name="groupScheduleId">Group schedule id.</param>
        /// <returns>Parsed group schedule.</returns>
        public async Task<RozKpiApiGroupSchedule> GetGroupSchedule(Guid groupScheduleId)
        {
            var schedulePage = await GetSchedulePage(groupScheduleId, RozKpiApiScheduleType.GROUP);

            var parsedSchedule = scheduleParser.Parse(schedulePage.DocumentNode);
            parsedSchedule.ScheduleId = groupScheduleId;

            return parsedSchedule;
        }
    }
}
