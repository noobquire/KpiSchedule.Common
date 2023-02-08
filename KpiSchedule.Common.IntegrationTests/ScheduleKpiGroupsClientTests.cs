using FluentAssertions;
using KpiSchedule.Common.Clients.RozKpiApi;
using KpiSchedule.Common.Models.ScheduleKpiApi;
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
        private ScheduleKpiGroupsClient client => serviceProvider.GetRequiredService<ScheduleKpiGroupsClient>();

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            this.serviceProvider = new ServiceCollection()
                .AddSerilogConsoleLogger()
                .AddKpiClient<ScheduleKpiGroupsClient>(config)
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
