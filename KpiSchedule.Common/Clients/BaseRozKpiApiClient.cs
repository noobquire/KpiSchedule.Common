using HtmlAgilityPack;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Parsers.ScheduleGroupSelection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KpiSchedule.Common.Clients
{
    public class BaseRozKpiApiClient : BaseClient
    {
        protected HttpClient client;
        protected string formValidationKeyValue;
        protected readonly FormValidationParser formValidationParser;

        public BaseRozKpiApiClient(ILogger logger, FormValidationParser formValidationParser) : base(logger)
        {
            this.formValidationParser = formValidationParser;
        }

        /// <summary>
        /// Get group/teacher schedule page using schedule id.
        /// </summary>
        /// <param name="scheduleId">Schedule id.</param>
        /// <param name="scheduleType">Schedule type key: v for teacher, g for group</param>
        /// <returns>Schedule HTML document.</returns>
        public async Task<HtmlDocument> GetSchedulePage(Guid scheduleId, string scheduleType)
        {
            string requestApi = $"ViewSchedule.aspx?{scheduleType}={scheduleId}";

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
    }
}
