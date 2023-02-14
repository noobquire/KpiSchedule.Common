using HtmlAgilityPack;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.RozKpiApi;
using KpiSchedule.Common.Parsers.ScheduleGroupSelection;
using KpiSchedule.Common.Parsers.TeacherSchedulePage;
using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static KpiSchedule.Common.Clients.RozKpiApiClientConstants;

namespace KpiSchedule.Common.Clients
{
    public class RozKpiApiTeachersClient : BaseRozKpiApiClient
    {
        private TeacherSchedulePageParser scheduleParser;

        public RozKpiApiTeachersClient(
            IHttpClientFactory clientFactory,
            ILogger logger,
            FormValidationParser formValidationParser,
            TeacherSchedulePageParser scheduleParser) : base(logger, formValidationParser)
        {
            client = clientFactory.CreateClient(nameof(RozKpiApiTeachersClient));
            client.DefaultRequestHeaders.Host = client.BaseAddress.Host;
            formValidationKeyValue = GetFormEventValidation().Result;
            formValidationKeyValue = GetFormEventValidation().Result;
            this.scheduleParser = scheduleParser;
        }

        private async Task<string> GetFormEventValidation()
        {
            var teacherSelectionPage = await GetTeacherSelectionPage();

            return formValidationParser.Parse(teacherSelectionPage.DocumentNode);
        }

        /// <summary>
        /// Get teacher schedule selection HTML page.
        /// </summary>
        /// <returns>Schedule group selection HTML page.</returns>
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


        /// <summary>
        /// Get list of teacher names starting with specified prefix.
        /// </summary>
        /// <param name="teacherNamePrefix">Teacher name prefix.</param>
        /// <returns>List of teachers with specified name prefix.</returns>
        /// <exception cref="KpiScheduleClientException">Unable to deserialize response.</exception>
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

        /// <summary>
        /// Get teacher schedule ID for given teacher name.
        /// </summary>
        /// <param name="teacherName">Teacher name.</param>
        /// <returns>Teacher schedule id.</returns>
        public async Task<Guid> GetTeacherScheduleId(string teacherName)
        {
            string requestApi = "ScheduleGroupSelection.aspx";

            var requestDictionary = new Dictionary<string, string>()
            {
                [FORM_EVENT_VALIDATION_KEY] = formValidationKeyValue,
                [FORM_SHOW_SCHEDULE_KEY] = FORM_SHOW_SCHEDULE_VALUE,
                [FORM_GROUP_NAME_KEY] = teacherName,
                [FORM_EVENT_TARGET_KEY] = string.Empty
            };

            var request = new FormUrlEncodedContent(requestDictionary);

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
            var viewSchedulePrefix = "/Schedules/ViewSchedule.aspx?g=";
            var scheduleIdStr = location.Substring(viewSchedulePrefix.Length);

            return new Guid(scheduleIdStr);
        }

        private bool IsTeacherNotFoundPage(HtmlNode documentNode)
        {
            return documentNode.InnerHtml.Contains("Викладача з такими даними не знайдено!");
        }

        /// <summary>
        /// Get parsed <see cref="RozKpiApiTeacherSchedule"/> for given teacher schedule id.
        /// </summary>
        /// <param name="groupScheduleId">Teacher schedule id.</param>
        /// <returns>Parsed teacher schedule.</returns>
        public async Task<RozKpiApiTeacherSchedule> GetTeacherSchedule(Guid teacherScheduleId)
        {
            var schedulePage = await GetSchedulePage(teacherScheduleId, RozKpiApiScheduleType.TEACHER);

            var parsedSchedule = scheduleParser.Parse(schedulePage.DocumentNode);
            parsedSchedule.ScheduleId = teacherScheduleId;

            return parsedSchedule;
        }
    }
}
