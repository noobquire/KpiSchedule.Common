using KpiSchedule.Common.Clients;
using KpiSchedule.Common.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KpiSchedule.Common.IntegrationTests
{
    [TestFixture]
    internal class ScheduleKpiClientTests
    {
        private IServiceProvider serviceProvider;
        private ScheduleKpiApiClient client => serviceProvider.GetRequiredService<ScheduleKpiApiClient>();

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            this.serviceProvider = new ServiceCollection()
                .AddSerilogConsoleLogger()
                .AddKpiClient<ScheduleKpiApiClient>(config)
                .BuildServiceProvider();
        }

        [Test]
        public async Task GetAllGroups_ShouldReturnGroupsList()
        {
            var groups = await client.GetAllGroups();

            Assert.IsNotEmpty(groups.Data);
        }

        [Test]
        public async Task GetAllLecturers_ShouldReturnLecturersList()
        {
            var teachers = await client.GetAllTeachers();

            Assert.IsNotEmpty(teachers.Data);
        }

        [Test]
        public async Task GetGroupSchedule_ShouldReturnGroupSchedule()
        {
            var schedule = await client.GetGroupSchedule("f4382a6b-269e-4cb7-86dd-8120a731b9df");

            Assert.IsNotNull(schedule.Data);
        }

        [Test]
        public async Task GetTimeInfo_ShouldReturnTimeInfo()
        {
            var timeInfo = await client.GetTimeInfo();

            Assert.IsNotNull(timeInfo.Data);
        }

        [Test]
        public async Task GetTeacherSchedule_GetTeacherSchedule()
        {
            var schedule = await client.GetTeacherSchedule("18bed22e-f86e-411c-b23d-ec246181569e");

            Assert.IsNotNull(schedule.Data);
        }
    }
}
