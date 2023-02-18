using HtmlAgilityPack;
using KpiSchedule.Common.Clients.Interfaces;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Parsers.ScheduleGroupSelection;
using Serilog;
using System.Net;

namespace KpiSchedule.Common.Clients
{
    public class BaseRozKpiApiClient : BaseClient, IBaseRozKpiClient
    {
        protected HttpClient client;
        protected string formValidationValue;
        protected readonly FormValidationParser formValidationParser;

        public BaseRozKpiApiClient(ILogger logger, FormValidationParser formValidationParser) : base(logger)
        {
            this.formValidationParser = formValidationParser;
        }

        /// <inheritdoc/>
        public async Task<HtmlDocument> GetSchedulePage(Guid scheduleId, RozKpiApiScheduleType type = RozKpiApiScheduleType.GroupSchedule)
        {
            var scheduleParamKey = GetScheduleTypeKey(type);
            string requestApi = $"ViewSchedule.aspx?{scheduleParamKey}={scheduleId}";

            var response = await client.GetAsync(requestApi);

            var errorLocation = "/Error.aspx?aspxerrorpath=/Schedules/ViewSchedule.aspx";
            if (response.StatusCode == HttpStatusCode.Redirect && response.Headers.Location.ToString() == errorLocation)
            {
                logger.Error("Schedule ID {scheduleId} exists, but schedule page does not exist", scheduleId);
                throw new KpiScheduleClientGroupNotFoundException("Group with requested name was not found.");
            }

            var requestUrl = response.RequestMessage.RequestUri.ToString();
            await CheckIfSuccessfulResponse(response, requestUrl);
            await CheckIfResponseBodyIsNullOrEmpty(response, requestUrl);

            var responseHtml = await response.Content.ReadAsStringAsync();
            var document = new HtmlDocument();
            document.LoadHtml(responseHtml);

            return document;
        }

        protected bool IsGroupNotFoundPage(HtmlNode documentNode)
        {
            return documentNode.InnerHtml.Contains("Групи з такою назвою не знайдено!");
        }

        private string GetScheduleTypeKey(RozKpiApiScheduleType type) => type switch
        {
            RozKpiApiScheduleType.GroupSchedule => "g",
            RozKpiApiScheduleType.TeacherSchedule => "v",
            _ => "g"
        };
    }
}
