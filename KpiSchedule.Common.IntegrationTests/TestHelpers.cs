using KpiSchedule.Common.Clients;
using KpiSchedule.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using System.Net;

namespace KpiSchedule.Common.IntegrationTests
{
    public static class TestHelpers
    {
        /// <summary>
        /// Because roz.kpi.ua API is often unresponsive, this service collection extension adds 
        /// a mock http client which returns saved responses from files.
        /// </summary>
        public static IServiceCollection AddMockRozKpiApiClients(this IServiceCollection services, IConfiguration config)
        {
            var clientConfiguration = config.GetSection(typeof(RozKpiApiGroupsClient).Name).Get<KpiApiClientConfiguration>();

            var mockHttpHandler = SetupMockHttpHandler();

            services.AddHttpClient(typeof(RozKpiApiGroupsClient).Name, c =>
            {
                c.BaseAddress = new Uri(clientConfiguration.Url);
                c.Timeout = TimeSpan.FromSeconds(clientConfiguration.TimeoutSeconds);
            }).ConfigurePrimaryHttpMessageHandler(() => mockHttpHandler);

            services.AddHttpClient(typeof(RozKpiApiTeachersClient).Name, c =>
            {
                c.BaseAddress = new Uri(clientConfiguration.Url);
                c.Timeout = TimeSpan.FromSeconds(clientConfiguration.TimeoutSeconds);
            }).ConfigurePrimaryHttpMessageHandler(() => mockHttpHandler);

            services.AddScoped<RozKpiApiTeachersClient>();
            services.AddScoped<RozKpiApiGroupsClient>();

            return services;
        }

        /// <summary>
        /// Mock roz.kpi.ua responses when it is down.
        /// </summary>
        private static MockHttpMessageHandler SetupMockHttpHandler()
        {
            var mockHandler = new MockHttpMessageHandler();

            // Get groups
            var groupsResponse = File.ReadAllText("TestData/RozKpiApiResponses/groups.json");
            mockHandler
                .When(HttpMethod.Post, "http://epi.kpi.ua/Schedules/ScheduleGroupSelection.aspx/GetGroups")
                .Respond("application/json", groupsResponse);

            // Get group schedule
            var groupScheduleResponse = File.ReadAllText("TestData/RozKpiApiResponses/group-schedule-page.html");
            mockHandler
                .When(HttpMethod.Get, "http://epi.kpi.ua/Schedules/ViewSchedule.aspx?g=13623e82-3f89-4815-b475-df7f442832e6")
                .Respond("text/html", groupScheduleResponse);

            // Get teachers
            var teachersResponse = File.ReadAllText("TestData/RozKpiApiResponses/teachers.json");
            mockHandler
                .When(HttpMethod.Post, "http://epi.kpi.ua/Schedules/LecturerSelection.aspx/GetLecturers")
                .Respond("application/json", teachersResponse);

            // Get group selection page 
            var groupSelectionResponse = File.ReadAllText("TestData/RozKpiApiResponses/group-selection-page.html");
            mockHandler
                .When(HttpMethod.Get, "http://epi.kpi.ua/Schedules/ScheduleGroupSelection.aspx")
                .Respond("text/html", groupSelectionResponse);

            // Get teacher selection page
            var teacherSelectionResponse = File.ReadAllText("TestData/RozKpiApiResponses/lecturer-selection-page.html");
            mockHandler
                .When(HttpMethod.Get, "http://epi.kpi.ua/Schedules/LecturerSelection.aspx")
                .Respond("text/html", teacherSelectionResponse);

            // Get teacher schedule
            var teacherScheduleResponse = File.ReadAllText("TestData/RozKpiApiResponses/lecturer-schedule-page.html");
            mockHandler
                .When(HttpMethod.Get, "http://epi.kpi.ua/Schedules/ViewSchedule.aspx?v=de47207f-8e13-4747-8654-9d29f7d01e89")
                .Respond("text/html", teacherScheduleResponse);

            // Get group name conflict
            var groupNameConflictResponse = File.ReadAllText("TestData/RozKpiApiResponses/group-selection-name-conflict-page.html");
            mockHandler
                .When(HttpMethod.Post, "http://epi.kpi.ua/Schedules/ScheduleGroupSelection.aspx")
                .WithHeaders(RozKpiApiClientConstants.FORM_GROUP_NAME_KEY, "БМ-01")
                .Respond("text/html", groupNameConflictResponse);

            // Get schedule with unparsable table
            var extramuralGroupScheduleResponse = File.ReadAllText("TestData/RozKpiApiResponses/extramural-group-schedule-page.html");
            mockHandler
                .When(HttpMethod.Get, "http://epi.kpi.ua/Schedules/ViewSchedule.aspx?g=2d3e0d7f-2cf9-488a-8b94-a82e5798cfe2")
                .Respond("text/html", extramuralGroupScheduleResponse);
            
            // Get schedule id
            mockHandler
                .When(HttpMethod.Post, "http://epi.kpi.ua/Schedules/ScheduleGroupSelection.aspx")
                .WithHeaders(RozKpiApiClientConstants.FORM_GROUP_NAME_KEY, "ІТ-04")
                .Respond(HttpStatusCode.Redirect)
                .WithHeaders("Location", "/Schedules/ViewSchedule.aspx?g=13623e82-3f89-4815-b475-df7f442832e6");
            
            return mockHandler;
        }
    }
}
