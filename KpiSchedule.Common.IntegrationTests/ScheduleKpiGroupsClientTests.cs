using FluentAssertions;
using KpiSchedule.Common.Clients;
using KpiSchedule.Common.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace KpiSchedule.Common.IntegrationTests
{
    [TestFixture]
    internal class ScheduleKpiGroupsClientTests
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
    }
}
