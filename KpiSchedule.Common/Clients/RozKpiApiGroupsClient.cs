using Serilog;
using System.Text.Json;
using KpiSchedule.Common.Exceptions;
using System.Text;
using HtmlAgilityPack;
using static KpiSchedule.Common.Clients.RozKpiApiClientConstants;
using KpiSchedule.Common.Parsers.ScheduleGroupSelection;
using System.Net;
using KpiSchedule.Common.Parsers.GroupSchedulePage;
using KpiSchedule.Common.Clients.Interfaces;
using KpiSchedule.Common.Models.RozKpiApi.Group;
using KpiSchedule.Common.Models.RozKpiApi.Base;

namespace KpiSchedule.Common.Clients
{
    /// <summary>
    /// Client for pulling group schedules data from roz.kpi.ua.
    /// </summary>
    public class RozKpiApiGroupsClient : BaseRozKpiApiClient, IRozKpiApiGroupsClient
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
            formValidationValue = GetFormEventValidation().Result;
            this.conflictingGroupNamesParser = conflictingGroupNamesParser;
            this.scheduleParser = scheduleParser;
        }

        /// <inheritdoc/>
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
                groups.Data = Enumerable.Empty<string>().ToList();
            }

            groups.GroupPrefix = groupPrefix;
            return groups;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<IEnumerable<Guid>> GetGroupScheduleIds(string groupName)
        {
            string requestApi = "ScheduleGroupSelection.aspx";

            var requestDictionary = new Dictionary<string, string>()
            {
                [FORM_EVENT_VALIDATION_KEY] = formValidationValue,
                [FORM_SHOW_GROUP_SCHEDULE_KEY] = FORM_SHOW_SCHEDULE_VALUE,
                [FORM_GROUP_NAME_KEY] = groupName,
                [FORM_EVENT_TARGET_KEY] = string.Empty
            };

            var request = new FormUrlEncodedContent(requestDictionary);

            logger.Information("Getting scheduleId for group {groupName}", groupName);
            var response = await client.PostAsync(requestApi, request);

            var requestUrl = response.RequestMessage.RequestUri.ToString();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // we got group names conflict or group not found page
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

        /// <inheritdoc/>
        public async Task<RozKpiApiGroupSchedule> GetGroupSchedule(Guid groupScheduleId)
        {
            var schedulePage = await GetSchedulePage(groupScheduleId, RozKpiApiScheduleType.GroupSchedule);

            var parsedSchedule = scheduleParser.Parse(schedulePage.DocumentNode);
            parsedSchedule.ScheduleId = groupScheduleId;

            return parsedSchedule;
        }
    }
}
