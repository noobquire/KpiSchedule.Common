using HtmlAgilityPack;
using KpiSchedule.Common.Clients.Interfaces;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.RozKpiApi;
using KpiSchedule.Common.Parsers.ScheduleGroupSelection;
using KpiSchedule.Common.Parsers.TeacherSchedulePage;
using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;
using static KpiSchedule.Common.Clients.RozKpiApiClientConstants;

namespace KpiSchedule.Common.Clients
{
    /// <summary>
    /// Client for pulling teacher schedules data from roz.kpi.ua.
    /// </summary>
    public class RozKpiApiTeachersClient : BaseRozKpiApiClient, IRozKpiApiTeachersClient
    {
        private TeacherSchedulePageParser scheduleParser;

        /// <summary>
        /// Initialize a new instance of <see cref="RozKpiApiTeachersClient"/> class.
        /// </summary>
        /// <param name="clientFactory">HTTP client factory.</param>
        /// <param name="logger">Logging interface.</param>
        /// <param name="formValidationParser">Form validation field parser.</param>
        /// <param name="scheduleParser">Teacher schedules parser.</param>
        public RozKpiApiTeachersClient(
            IHttpClientFactory clientFactory,
            ILogger logger,
            FormValidationParser formValidationParser,
            TeacherSchedulePageParser scheduleParser) : base(logger, formValidationParser)
        {
            client = clientFactory.CreateClient(nameof(RozKpiApiTeachersClient));
            client.DefaultRequestHeaders.Host = client.BaseAddress.Host;
            formValidationValue = GetFormEventValidation().Result;
            formValidationValue = GetFormEventValidation().Result;
            this.scheduleParser = scheduleParser;
        }

        private async Task<string> GetFormEventValidation()
        {
            var teacherSelectionPage = await GetTeacherSelectionPage();

            return formValidationParser.Parse(teacherSelectionPage.DocumentNode);
        }

        /// <inheritdoc/>
        public async Task<HtmlDocument> GetTeacherSelectionPage()
        {
            string requestApi = "LecturerSelection.aspx";

            var response = await client.GetAsync(requestApi);

            var requestUrl = response.RequestMessage.RequestUri.ToString();
            await CheckIfSuccessfulResponse(response, requestUrl);
            await CheckIfResponseBodyIsNullOrEmpty(response, requestUrl);

            var responseHtml = await response.Content.ReadAsStringAsync();
            var document = new HtmlDocument();
            document.LoadHtml(responseHtml);

            return document;
        }


        /// <inheritdoc/>
        public async Task<RozKpiApiTeachersList> GetTeachers(string teacherNamePrefix)
        {
            string requestApi = "LecturerSelection.aspx/GetLecturers";
            var request = new BaseRozKpiApiRequest(teacherNamePrefix);
            var requestJson = JsonSerializer.Serialize(request);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(requestApi, requestContent);

            var teachers = await VerifyAndParseResponseBody<RozKpiApiTeachersList>(response);
            if (teachers.Data is null)
            {
                teachers.Data = Array.Empty<string>();
            }

            teachers.TeacherNamePrefix = teacherNamePrefix;
            return teachers;
        }

        /// <inheritdoc/>
        public async Task<Guid> GetTeacherScheduleId(string teacherName)
        {
            string requestApi = "LecturerSelection.aspx";

            var requestDictionary = new Dictionary<string, string>()
            {
                [FORM_EVENT_VALIDATION_KEY] = formValidationValue,
                [FORM_SHOW_TEACHER_SCHEDULE_KEY] = FORM_SHOW_SCHEDULE_VALUE,
                [FORM_TEACHER_NAME_KEY] = teacherName,
                [FORM_EVENT_TARGET_KEY] = string.Empty
            };

            var request = new FormUrlEncodedContent(requestDictionary);

            logger.Information("Getting scheduleId for teacher {teacherName}", teacherName);
            var response = await client.PostAsync(requestApi, request);

            var requestUrl = response.RequestMessage.RequestUri.ToString();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseHtml = await response.Content.ReadAsStringAsync();
                var document = new HtmlDocument();
                document.LoadHtml(responseHtml);

                if (IsTeacherNotFoundPage(document.DocumentNode))
                {
                    logger.Error("Teacher {teacherName} not found", teacherName);
                    throw new KpiScheduleClientGroupNotFoundException("Teacher with requested name was not found.");
                }
            }

            await CheckIfExpectedResponseCode(response, HttpStatusCode.Redirect, requestUrl);

            var location = response.Headers.Location.ToString();
            var viewSchedulePrefix = "/Schedules/ViewSchedule.aspx?v=";
            var scheduleIdStr = location.Substring(viewSchedulePrefix.Length);

            return new Guid(scheduleIdStr);
        }

        private bool IsTeacherNotFoundPage(HtmlNode documentNode)
        {
            return documentNode.InnerHtml.Contains("Викладача з такими даними не знайдено!");
        }

        /// <inheritdoc/>
        public async Task<RozKpiApiTeacherSchedule> GetTeacherSchedule(Guid teacherScheduleId)
        {
            var schedulePage = await GetSchedulePage(teacherScheduleId, RozKpiApiScheduleType.TeacherSchedule);

            var parsedSchedule = scheduleParser.Parse(schedulePage.DocumentNode);
            parsedSchedule.ScheduleId = teacherScheduleId;

            return parsedSchedule;
        }
    }
}
