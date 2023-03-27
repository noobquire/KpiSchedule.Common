using FluentAssertions;
using KpiSchedule.Common.Clients;
using KpiSchedule.Common.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;

namespace KpiSchedule.Common.IntegrationTests
{
    [TestFixture]
    public class RozKpiTeachersClientTests
    {
        private IServiceProvider serviceProvider;
        private RozKpiApiTeachersClient client => serviceProvider.GetRequiredService<RozKpiApiTeachersClient>();

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            bool mockRozKpiApiResponses = config.GetSection("MockRozKpiApiResponses").Get<bool>();
            var services = new ServiceCollection()
                .AddSerilogConsoleLogger(LogEventLevel.Information)
                .AddRozKpiParsers();

            if (mockRozKpiApiResponses)
            {
                services.AddMockRozKpiApiClients(config);
            }
            else
            {
                services.AddKpiClient<RozKpiApiTeachersClient>(config);
            }

            serviceProvider = services.BuildServiceProvider();
        }

        [Test]
        public async Task GetTeachers_WithValidGroupRequest_ShouldReturnTeachersList()
        {
            var teacherNamePrefix = "А";

            var teachers = await client.GetTeachers(teacherNamePrefix);

            Assert.IsNotEmpty(teachers.Data);
            Assert.True(teachers.Data.All(g => g.StartsWith(teacherNamePrefix)));
        }

        [Test]
        public async Task GetTeacherSchedule_ShouldReturnTeacherSchedule()
        {
            var scheduleId = await client.GetTeacherScheduleId("Лісовиченко Олег Іванович");

            var teacherSchedule = await client.GetTeacherSchedule(scheduleId);

            teacherSchedule.TeacherName.Should().Contain("Лісовиченко Олег Іванович");
        }
    }
}
