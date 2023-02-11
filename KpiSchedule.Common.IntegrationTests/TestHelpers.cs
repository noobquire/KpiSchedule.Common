using KpiSchedule.Common.Clients;
using KpiSchedule.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace KpiSchedule.Common.IntegrationTests
{
    public static class TestHelpers
    {
        /// <summary>
        /// Because roz.kpi.ua API is often unresponsive, this service collection extension adds 
        /// a mock http client which returns saved responses from files.
        /// </summary>
        public static IServiceCollection AddMockRozKpiApiClient(this IServiceCollection services, IConfiguration config)
        {
            var clientConfiguration = config.GetSection(typeof(RozKpiApiClient).Name).Get<KpiApiClientConfiguration>();

            var mockHttpHandler = SetupMockHttpHandler();

            services.AddHttpClient(typeof(RozKpiApiClient).Name, c =>
            {
                c.BaseAddress = new Uri(clientConfiguration.Url);
                c.Timeout = TimeSpan.FromSeconds(clientConfiguration.TimeoutSeconds);
            }).ConfigurePrimaryHttpMessageHandler(() => mockHttpHandler);

            services.AddScoped<RozKpiApiClient>();

            return services;
        }

        private static MockHttpMessageHandler SetupMockHttpHandler()
        {
            var mockHandler = new MockHttpMessageHandler();

            // Get groups
            var groupsResponse = File.ReadAllText("RozKpiApiResponses/groups.json");
            mockHandler
                .When(HttpMethod.Post, "http://epi.kpi.ua/Schedules/ScheduleGroupSelection.aspx/GetGroups")
                .Respond("application/json", groupsResponse);

            // Get group schedule
            var groupScheduleResponse = File.ReadAllText("RozKpiApiResponses/group-schedule-page.html");
            mockHandler
                .When(HttpMethod.Post, "http://epi.kpi.ua/Schedules/ScheduleGroupSelection.aspx")
                .Respond("text/html", groupScheduleResponse);

            // Get teachers
            var teachersResponse = File.ReadAllText("RozKpiApiResponses/teachers.json");
            mockHandler
                .When(HttpMethod.Post, "http://epi.kpi.ua/Schedules/LecturerSelection.aspx/GetLecturers")
                .Respond("application/json", teachersResponse);

            // Get group selection page
            var groupSelectionResponse = File.ReadAllText("RozKpiApiResponses/group-selection-page.html");
            mockHandler
                .When(HttpMethod.Get, "http://epi.kpi.ua/Schedules/ScheduleGroupSelection.aspx")
                .Respond("text/html", groupSelectionResponse);

            // Get teacher selection page
            var teacherSelectionResponse = File.ReadAllText("RozKpiApiResponses/lecturer-selection-page.html");
            mockHandler
                .When(HttpMethod.Get, "http://epi.kpi.ua/Schedules/LecturerSelection.aspx")
                .Respond("text/html", teacherSelectionResponse);

            // Get teacher schedule
            var teacherScheduleResponse = File.ReadAllText("RozKpiApiResponses/lecturer-schedule-page.html");
            mockHandler
                .When(HttpMethod.Post, "http://epi.kpi.ua/Schedules/LecturerSelection.aspx")
                .Respond("text/html", teacherScheduleResponse);

            return mockHandler;
        }
    }
}
